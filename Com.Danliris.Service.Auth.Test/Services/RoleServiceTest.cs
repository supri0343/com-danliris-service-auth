using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.Test.DataUtils;
using Com.Danliris.Service.Auth.Test.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Test.Services
{
    public class RoleServiceTest : BaseServiceTest<Role, RoleViewModel, RoleService, RoleDataUtil>
    {
        public RoleServiceTest() : base("Role")
        {
        }

        protected override Role OnUpdating(Role model)
        {
            model.Name += "[updated]";
            return model;
        }
    }
}
