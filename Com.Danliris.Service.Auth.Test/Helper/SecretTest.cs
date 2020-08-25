using Com.Danliris.Service.Auth.WebApi.Utilities;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Auth.Test.Helper
{
    public class SecretTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
           
            Secret secret = new Secret(configurationMock.Object);

            Assert.NotNull(secret);
        }
    }
}
