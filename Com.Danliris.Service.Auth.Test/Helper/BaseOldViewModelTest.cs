using Com.Danliris.Service.Auth.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Auth.Test.Helper
{
   public class BaseOldViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            BaseOldViewModel viewModel = new BaseOldViewModel()
            {
                _active = true,
                _createAgent = "_createAgent",
                _createdBy = "_createdBy",
                _createdDate = DateTime.Now,
                _deleted =true,
                _updateAgent = "_updateAgent",
                _updatedBy = "_updatedBy",
                _updatedDate =DateTime.Now
            };

            Assert.NotNull(viewModel);

        }
    }
}
