using AutoMapper;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Services.IdentityService;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.Test.DataUtils;
using Com.Danliris.Service.Auth.WebApi.Controllers.v1;
using Com.Danliris.Service.Auth.WebApi.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Auth.Test.Controllers
{
    public class AuthenticateControllerTest
    {
        public (Mock<IAccountService> Service, Mock<IMapper> Mapper, Mock<ISecret> Secret) GetMocks()
        {
            return (Service: new Mock<IAccountService>(), Mapper: new Mock<IMapper>(), Secret: new Mock<ISecret>());
        }

        public AuthenticateController GetController((Mock<IAccountService> Service, Mock<IMapper> Mapper, Mock<ISecret> Secret) mocks)
        {
            AuthenticateController controller = (AuthenticateController)Activator.CreateInstance(typeof(AuthenticateController), mocks.Service.Object, mocks.Mapper.Object, mocks.Secret.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {   
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        public Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });


            return serviceProvider;
        }

        public int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        [Fact]
        public async Task Post_WithoutException_ReturnToken(){
            var mocks = GetMocks();
            RoleDataUtil roleDataUtil = new RoleDataUtil();
            AccountDataUtil accountDataUtil = new AccountDataUtil();
            var roleModel = roleDataUtil.GetNewData();
            var accountModel = accountDataUtil.GetNewData();
            accountModel.AccountRoles.Add(new AccountRole(){
                Role = roleModel,
                RoleUId = "RoleUId",
                UId = "UId"
            });
            mocks.Secret.Setup(s => s.SecretString).Returns("secretsecretsecret");
            mocks.Service.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(accountModel);
            var roleVM = roleDataUtil.GetNewViewModel();
            var accountVM = accountDataUtil.GetNewViewModel();
            accountVM.roles.Add(roleVM);
            mocks.Mapper.Setup(s => s.Map<AccountViewModel>(It.IsAny<Account>())).Returns(accountVM);
            AuthenticateController controller = GetController(mocks);
            var result = await controller.Post(new LoginViewModel(){
                Password = "password",
                Username = "username"
            });
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_WithoutException_ReturnNotFound()
        {

            var mocks = GetMocks();
            
            mocks.Service.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(default(Account));
           
            AuthenticateController controller = GetController(mocks);
            var result = await controller.Post(new LoginViewModel()
            {
                Password = "password",
                Username = "username"
            });
            Assert.Equal((int)HttpStatusCode.NotFound, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_WithException_ReturnBadRequest()
        {

            var mocks = GetMocks();
            mocks.Service.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("error"));
            
            AuthenticateController controller = GetController(mocks);
            var result = await controller.Post(new LoginViewModel()
            {
                Password = "password",
                Username = "username"
            });
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));
        }
    }
}
