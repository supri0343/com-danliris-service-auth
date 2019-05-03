using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Services.IdentityService;
using Com.Danliris.Service.Auth.Lib.Utilities;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Auth.Lib.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private const string UserAgent = "auth-service";
        protected DbSet<Account> DbSet;
        protected IIdentityService IdentityService;
        public AuthDbContext DbContext;

        public AccountService(IServiceProvider serviceProvider, AuthDbContext dbContext)
        {
            DbContext = dbContext;
            this.DbSet = dbContext.Set<Account>();
            this.IdentityService = serviceProvider.GetService<IIdentityService>();
        }
        public async Task<int> CreateAsync(Account model)
        {
            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
            EntityExtension.FlagForCreate(model.AccountProfile, IdentityService.Username, UserAgent);
            foreach (var item in model.AccountRoles)
            {
                item.Role = null;
                EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
            }
            model.Password = model.Password.Sha256();
            DbSet.Add(model);

            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            Account model = await ReadByIdAsync(id);
            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            EntityExtension.FlagForDelete(model.AccountProfile, IdentityService.Username, UserAgent, true);
            foreach (var item in model.AccountRoles)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
            }
            DbSet.Update(model);
            return await DbContext.SaveChangesAsync();

        }

        public ReadResponse<Account> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<Account> query = DbSet.Where(x => !x.IsDeleted);

            List<string> searchAttributes = new List<string>()
            {
                "Username"
            };

            query = QueryHelper<Account>.Search(query, searchAttributes, keyword);

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<Account>.Filter(query, filterDictionary);

            List<string> selectedFields = new List<string>()
                {
                    "_id", "Username","profile",
                };

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<Account>.Order(query, orderDictionary);

            query = query.Select(x => new Account()
            {

                AccountProfile = x.AccountProfile,
                AccountRoles = x.AccountRoles,
                Active = x.Active,
                CreatedAgent = x.CreatedAgent,
                CreatedBy = x.CreatedBy,
                CreatedUtc = x.CreatedUtc,
                DeletedAgent = x.DeletedAgent,
                DeletedBy = x.DeletedBy,
                DeletedUtc = x.DeletedUtc,
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                IsLocked = x.IsLocked,
                LastModifiedAgent = x.LastModifiedAgent,
                LastModifiedBy = x.LastModifiedBy,
                LastModifiedUtc = x.LastModifiedUtc,
                Username = x.Username
            });

            Pageable<Account> pageable = new Pageable<Account>(query, page - 1, size);
            List<Account> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            return new ReadResponse<Account>(data, totalData, orderDictionary, selectedFields);
        }

        public async Task<Account> ReadByIdAsync(int id)
        {
            var result = await DbSet
                .Include(x => x.AccountProfile)
                .Include(x => x.AccountRoles)
                    .ThenInclude(i => i.Role)
                    .ThenInclude(y => y.Permissions)
                .FirstOrDefaultAsync(d => d.Id.Equals(id) && !d.IsDeleted);
            return result;
        }

        public async Task<int> UpdateAsync(int id, Account model)
        {
            var data = await ReadByIdAsync(id);

            data.Username = model.Username;
            if (!string.IsNullOrEmpty(model.Password))
                data.Password = model.Password.Sha256();
            data.IsLocked = model.IsLocked;
            data.AccountProfile.Dob = model.AccountProfile.Dob;
            data.AccountProfile.Email = model.AccountProfile.Email;
            data.AccountProfile.Firstname = model.AccountProfile.Firstname;
            data.AccountProfile.Gender = model.AccountProfile.Gender;
            data.AccountProfile.Lastname = model.AccountProfile.Lastname;

            var updatedRoles = model.AccountRoles.Where(x => data.AccountRoles.Any(y => y.RoleId == x.RoleId));
            var addedRoles = model.AccountRoles.Where(x => !data.AccountRoles.Any(y => y.RoleId == x.RoleId));
            var deletedRoles = data.AccountRoles.Where(x => !model.AccountRoles.Any(y => y.RoleId == x.RoleId));

            foreach (var item in updatedRoles)
            {
                var role = data.AccountRoles.SingleOrDefault(x => x.RoleId == item.RoleId);

                EntityExtension.FlagForUpdate(role, IdentityService.Username, UserAgent);
            }

            foreach (var item in addedRoles)
            {
                item.AccountId = id;
                item.Role = null;
                EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
                data.AccountRoles.Add(item);
            }

            foreach (var item in deletedRoles)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
            }

            EntityExtension.FlagForUpdate(data, IdentityService.Username, UserAgent);

            DbSet.Update(data);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<Account> Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("Username is required");
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password is required");
            }
            else
            {
                var user = await DbSet
                            .Include(x => x.AccountProfile)
                            .Include(x => x.AccountRoles)
                                .ThenInclude(i => i.Role)
                                .ThenInclude(y => y.Permissions)
                            .SingleOrDefaultAsync(d => d.Username.Equals(username) && d.Password.Equals(password.Sha256()) && !d.IsDeleted);

                return user;
            }
        }
    }
}
