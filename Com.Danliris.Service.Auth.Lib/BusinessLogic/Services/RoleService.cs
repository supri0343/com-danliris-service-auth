using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Services.IdentityService;
using Com.Danliris.Service.Auth.Lib.Utilities;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Auth.Lib.BusinessLogic.Services
{
    public class RoleService : IRoleService
    {
        private const string UserAgent = "auth-service";
        protected DbSet<Role> DbSet;
        protected IIdentityService IdentityService;
        public AuthDbContext DbContext;

        public RoleService(IServiceProvider serviceProvider, AuthDbContext dbContext)
        {
            DbContext = dbContext;
            this.DbSet = dbContext.Set<Role>();
            this.IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CreateAsync(Role model)
        {
            model.Code = await GenerateCode();
            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
            foreach(var item in model.Permissions)
            {
                item.permission = 1;
                EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
            }
            DbSet.Add(model);

            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            Role model = await ReadByIdAsync(id);
            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            foreach (var item in model.Permissions)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
            }
            DbSet.Update(model);
            return await DbContext.SaveChangesAsync();
        }

        public ReadResponse<Role> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<Role> query = DbSet.Include(x => x.Permissions).Where(x => !x.IsDeleted);

           List<string> searchAttributes = new List<string>()
            {
                "Code", "Name"
            };

            query = QueryHelper<Role>.Search(query, searchAttributes, keyword);

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<Role>.Filter(query, filterDictionary);

            List<string> selectedFields = new List<string>()
                {
                    "_id", "code", "name", "permissions","description"
                };

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<Role>.Order(query, orderDictionary);

            Pageable<Role> pageable = new Pageable<Role>(query, page - 1, size);
            List<Role> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            return new ReadResponse<Role>(data, totalData, orderDictionary, selectedFields);
        }

        public async Task<Role> ReadByIdAsync(int id)
        {
            var result = await DbSet
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsDeleted);

            return result;
        }

        public async Task<int> UpdateAsync(int id, Role model)
        {
            int Updated = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var data = await ReadByIdAsync(id);

                    //Add permission if it didt exist
                    foreach(var newPer in model.Permissions)
                    {
                        var existPer = data.Permissions.FirstOrDefault(a => a.Code == newPer.Code);

                        if(existPer == null)
                        {
                            newPer.RoleId = id;
                            newPer.permission = 1;
                            EntityExtension.FlagForCreate(newPer, IdentityService.Username, UserAgent);
                            data.Permissions.Add(newPer);
                        }
                    }

                    //Remove permission if uncheck
                    foreach (var oldPer in data.Permissions)
                    {
                        var existPer = model.Permissions.FirstOrDefault(a => a.Code == oldPer.Code);

                        if (existPer == null)
                        {
                            EntityExtension.FlagForDelete(oldPer, IdentityService.Username, UserAgent, true);
                        }
                    }

                    data.Code = model.Code;
                    data.Description = model.Description;
                    data.Name = model.Name;

                    EntityExtension.FlagForUpdate(data, IdentityService.Username, UserAgent);

                    DbSet.Update(data);
                    Updated =  await DbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return Updated;
        }

        public async Task<string>GenerateCode()
        {
            var lastId = DbSet.Where(x => x.IsDeleted == false).Count();

            var total = lastId + 1;

            var Code = "RL" + total.ToString().PadLeft(3,'0');

            return Code;
        }

        public bool CheckDuplicate(int id, string code)
        {
            return DbSet.Any(r => r.IsDeleted.Equals(false) && r.Id != id && r.Code.Equals(code));
        }
    }
}
