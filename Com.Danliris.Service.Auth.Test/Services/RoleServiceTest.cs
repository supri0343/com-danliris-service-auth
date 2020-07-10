using AutoMapper;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.Test.DataUtils;
using Com.Danliris.Service.Auth.Test.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Xunit;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;

namespace Com.Danliris.Service.Auth.Test.Services
{
    public class RoleServiceTest : BaseServiceTest<Role, RoleViewModel, RoleService, RoleDataUtil>
    {
        public RoleServiceTest() : base("Role")
        {
        }

        protected override Role OnUpdating(Role model)
        {
            model.Name += "[updated]";
            return model;
        }

        [Fact]
        public void Mapping_With_AutoMapper_Profiles()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Lib.AutoMapperProfiles.RoleProfile>();
                cfg.AddProfile<Lib.AutoMapperProfiles.AccountProfile>();
            });
            var mapper = configuration.CreateMapper();

            RoleViewModel vm = new RoleViewModel { _id = 1 };
            Role model = mapper.Map<Role>(vm);

            Assert.Equal(vm._id, model.Id);

        }

        [Fact]
        public async void UpdateAsync_When_addedRoles_Return_Success()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            var model = await _dataUtil(service).GetTestData();
            
            var Response = await service.UpdateAsync(model.Id, _dataUtil(service).GetDataInput());
            Assert.NotEqual(0, Response);
        }

        public override async void Should_Success_Validate_All_Null_Data()
        {
            var serviceProvider = GetServiceProvider();
            var service = GetService(serviceProvider.Object, _dbContext(GetCurrentMethod()));
            serviceProvider.Setup(s => s.GetService(typeof(IRoleService)))
                .Returns(service);
            var vm = new RoleViewModel();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(vm, serviceProvider.Object, null);
            Assert.True(vm.Validate(context).Count() > 0);

            var data = await _dataUtil(service).GetTestData();

            vm.code = data.Code;
            Assert.True(vm.Validate(context).Count() > 0);

            vm.permissions = new List<PermissionViewModel>()
            {
                new PermissionViewModel()
                {
                    unit = new UnitViewModel()
                }
            };
            Assert.True(vm.Validate(context).Count() > 0);
            vm.permissions = new List<PermissionViewModel>()
            {
                new PermissionViewModel()
                {
                    roleId = 1,
                    _createAgent = "a",
                    _createdBy = "a",
                    _createdDate = DateTime.UtcNow,
                    id = 1,
                    unit = new UnitViewModel()
                    {
                        Name = "err"
                    }
                }
            };
            Assert.True(vm.Validate(context).Count() > 0);
        }
    }
}
