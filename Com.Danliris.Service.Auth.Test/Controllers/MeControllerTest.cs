using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.Test.DataUtils;
using Com.Danliris.Service.Auth.WebApi.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Auth.Test.Controllers
{
    public class MeControllerTest
    {
        protected MeController GetController()
        {
            RoleDataUtil roleDataUtil = new RoleDataUtil();
            AccountDataUtil accountDataUtil = new AccountDataUtil();
            var roleVM = roleDataUtil.GetNewViewModel();
            var accountVM = accountDataUtil.GetNewViewModel();
            //accountVM.roles.Add(roleVM);
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername"),
                new Claim("profile", JsonConvert.SerializeObject(accountVM.profile)),
                //new Claim("permission", JsonConvert.SerializeObject(accountVM.roles)),
                new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };
            user.Setup(u => u.Claims).Returns(claims);
            MeController controller = (MeController)Activator.CreateInstance(typeof(MeController));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        public int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        [Fact]
        public void Get_Me_OK()
        {
            var controller = GetController();
            var result = controller.Me();

            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public void Get_Me_Exception()
        {
            var controller = GetController();
            controller.ControllerContext.HttpContext.User = null;
            var result = controller.Me();

            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }
    }
}
