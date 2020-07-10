using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Services.ValidateService;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Auth.Test.Helper
{
    public class ValidateServiceTest
    {
        [Fact]
        public void Should_Success_Validate()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            ValidateService validate = new ValidateService(serviceProviderMock.Object);
            Permission permission = new Permission()
            {
                Division = "Division"
            };
           
          validate.Validate(permission);
          
        }
    }
}
