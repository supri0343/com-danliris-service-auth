using Com.Danliris.Service.Auth.Lib.Services.ValidateService;
using Com.Danliris.Service.Auth.Lib.Utilities;
using Com.Danliris.Service.Auth.Lib.ViewModels;
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
        public void Should_Success_Instantiate()
        {
            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            ValidateService service = new ValidateService(serviceProviderMock.Object);
            UnitViewModel stage = new UnitViewModel()
            {
                Code = "Code",
                Division =new DivisionViewModel()
                {
                    Name ="Name"
                },
                Name ="Name"
            };
            service.Validate(stage);

        }
    }
}
