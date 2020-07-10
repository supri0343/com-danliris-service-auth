using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Auth.Test.Helper
{
   public class QueryHelperTest
    {
        [Fact]
        public void Filters_Return_Success()
        {
            List<Permission> data = new List<Permission>()
            {
                new Permission()
                {
                    UnitCode="UnitCode",
                    Division ="Division"
                }
            };
            Dictionary<string, object> filterDictionary = new Dictionary<string, object>();
            filterDictionary.Add("Division", "Division");
           
            var result = QueryHelper<Permission>.Filter(data.AsQueryable(), filterDictionary);
           
            Assert.True(0 < result.Count());
            Assert.NotNull(result);
        }

        [Fact]
        public void Order_Return_Success()
        {
            List<Permission> data = new List<Permission>()
            {
                new Permission()
                {
                    UnitCode="UnitCode",
                    Division ="Division"
                }
            };
            Dictionary<string, string> Order = new Dictionary<string, string>();
            Order.Add("Division", "desc");

            var result = QueryHelper<Permission>.Order(data.AsQueryable(), Order);
            Assert.NotNull(result);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void Search_Return_Success()
        {
            List<Permission> data = new List<Permission>()
            {
                new Permission()
                {
                    UnitCode="UnitCode",
                    Division ="Division"
                }
            };

            List<string> searchAttributes = new List<string>()
            {
                "Division",
            };

            var result = QueryHelper<Permission>.Search(data.AsQueryable(), searchAttributes, "Division", true);
            Assert.True(0 < result.Count());
            Assert.NotNull(result);
        }

    }
}
