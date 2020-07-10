using AutoMapper;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.WebApi.Utilities;
using Moq;
using System;
using System.Collections.Generic;
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

    }
}
