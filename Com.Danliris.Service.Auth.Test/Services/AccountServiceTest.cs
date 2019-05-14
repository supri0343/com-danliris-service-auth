using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.Test.DataUtils;
using Com.Danliris.Service.Auth.Test.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

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

        [Fact]
        public async void Should_Success_Authenticate_Data()
        {
            var service = GetService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var Response = service.Authenticate(model.Username, model.Password);
            Assert.NotNull(Response);
        }
    }
}
