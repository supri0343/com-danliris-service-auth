using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;
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
            //Setup
            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            ValidateService service = new ValidateService(serviceProviderMock.Object);
            UnitViewModel stage = new UnitViewModel()
            {
                Code = "Code",
                Division = new DivisionViewModel()
                {
                    Name = "Name"
                },
                Name = "Name"
            };

            //Act
            service.Validate(stage);

        }

        [Fact]
        public void Validate_Throws_ServiceValidationExeption()
        {
            //Setup
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            Mock<IRoleService> IRoleServiceMock = new Mock<IRoleService>();

            IRoleServiceMock.Setup(s => s.CheckDuplicate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            serviceProvider.Setup(S => S.GetService(typeof(IRoleService))).Returns(IRoleServiceMock.Object);

            RoleViewModel viewModel = new RoleViewModel();

            //Act
            var service = new Lib.Services.ValidateService.ValidateService(serviceProvider.Object);

            //Assert
            Assert.Throws<ServiceValidationException>(() => service.Validate(viewModel));

        }
    }
}
