using AutoMapper;
using Com.Danliris.Service.Auth.Lib.Utilities;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.WebApi.Utilities;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Xunit;


namespace Com.Danliris.Service.Auth.Test.Helper
{
    public class ResultFormatterTest
    {
        [Fact]
        public void Should_Success_OK_default()
        {
            List<DivisionViewModel> data = new List<DivisionViewModel>();
            Dictionary<string, string> order = new Dictionary<string, string>();
            order.Add("Name", "desc");
            List<string> select = new List<string>()
            {
                "Name"
            };
            Mock<IMapper> IMapperMock = new Mock<IMapper>();



            ResultFormatter result = new ResultFormatter("API 1", 200, "SUCCESS");
            var response =result.Ok<DivisionViewModel>(IMapperMock.Object, data, 1, 1, 1, 1, order, select);

            Assert.True(response.Count() > 0);
            Assert.NotNull(response);
        }

        [Fact]
        public void Fail_Return_Success()
        {
            //Setup
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";

            AccountProfileViewModel viewModel = new AccountProfileViewModel();
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(viewModel);

            var errorData = new
            {
                WarningError = "Format Not Match"
            };

            string error = JsonConvert.SerializeObject(errorData);
            var exception = new ServiceValidationException(validationContext, new List<ValidationResult>() { new ValidationResult(error, new List<string>() { "WarningError" }) });

            //Act
            var result = formatter.Fail(exception);

            //Assert
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void Fail_Throws_Exception()
        {
            //Setup
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";

            AccountViewModel viewModel = new AccountViewModel();
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(viewModel);
            var exception = new ServiceValidationException(validationContext, new List<ValidationResult>() { new ValidationResult("errorMessaage", new List<string>() { "WarningError" }) });

            //Act
            var result = formatter.Fail(exception);

            //Assert
            Assert.True(0 < result.Count());
        }

    }
}
