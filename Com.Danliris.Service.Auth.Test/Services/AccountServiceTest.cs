using AutoMapper;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.Test.DataUtils;
using Com.Danliris.Service.Auth.Test.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;


namespace Com.Danliris.Service.Auth.Test.Services
{
    public class AccountServiceTest : BaseServiceTest<Account, AccountViewModel, AccountService, AccountDataUtil>
    {
        public AccountServiceTest() : base("account")
        {
        }

        protected override Account OnUpdating(Account model)
        {
            model.Username += "[updated]";
            return model;
        }

        public override async void Should_Success_Create_Data()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var roleService = new RoleService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var roleDataUtil = new RoleDataUtil(roleService);
            var role = await roleDataUtil.GetTestData();
            var model = _dataUtil(service).GetNewData();
            model.AccountRoles = new List<AccountRole>()
            {
                new AccountRole()
                {
                    RoleId = role.Id
                }
            };
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async void Should_Success_Authenticate_Data()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var Response = service.Authenticate(model.Username, model.Password);
            Assert.NotNull(Response);
        }

        [Fact]
        public async void Should_Fail_Authenticate_Data_Username_null()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            await Assert.ThrowsAnyAsync<Exception>(() => service.Authenticate(null, model.Password));

        }

        [Fact]
        public async void Should_Fail_Authenticate_Data_Password_null()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            await Assert.ThrowsAnyAsync<Exception>(() => service.Authenticate(model.Password, null));

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

            AccountViewModel vm = new AccountViewModel { _id = 1 };
            Account model = mapper.Map<Account>(vm);

            Assert.Equal(vm._id, model.Id);

        }

        [Fact]
        public async void UpdateAsync_When_addedRoles_Return_Success(){
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod())); var model = await _dataUtil(service).GetTestData();
            var dataInput = new Account(){
                Username = "username",Password = "password",IsLocked = false,
                UId = "UId", AccountProfile = new AccountProfile(){
                    Dob = DateTimeOffset.UtcNow,
                    Email = "email",
                    Gender = "male",
                    Firstname = "firstname",
                    Lastname = "lastname",
                    UId = "UId",
                }, AccountRoles = new List<AccountRole>(){ new AccountRole(){
                        RoleId =2,
                        Role = new Role(){
                            Name="Name",UId="UId",Id =2, Description="Description"
                        }
                    }
                }
            };
            var Response = await service.UpdateAsync(model.Id, dataInput); Assert.NotEqual(0, Response);
        }

        public override async void Should_Success_Validate_All_Null_Data()
        {
            var serviceProvider = GetServiceProvider();
            var service = GetService(serviceProvider.Object, _dbContext(GetCurrentMethod()));
            serviceProvider.Setup(s => s.GetService(typeof(IAccountService)))
                .Returns(service);
            var vm = new AccountViewModel();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(vm, serviceProvider.Object, null);
            Assert.True(vm.Validate(context).Count() > 0);

            vm.username = "username";
            Assert.True(vm.Validate(context).Count() > 0);

            vm._id = 1;
            vm.password = "test";
            Assert.True(vm.Validate(context).Count() > 0);

            vm.profile = new AccountProfileViewModel()
            {
                id = 1,
                firstname = "test"
            };
            Assert.False(vm.Validate(context).Count() > 0);

            var data =await  _dataUtil(service).GetTestData();
            vm.username = data.Username;
            vm.password = data.Password;
            vm._id = data.Id + 1;

            Assert.True(vm.Validate(context).Count() > 0);

            vm.roles = new List<RoleViewModel>()
            {
                new RoleViewModel()
                {
                    _id = 1,
                },
                new RoleViewModel()
                {
                    _id = 1
                }
            };
            Assert.True(vm.Validate(context).Count() > 0);

            vm.roles = new List<RoleViewModel>()
            {
                new RoleViewModel()
            };
            Assert.True(vm.Validate(context).Count() > 0);
        }
    }
}
