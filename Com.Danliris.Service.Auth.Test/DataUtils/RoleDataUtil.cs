using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.Test.Utils;
using System.Collections.Generic;

namespace Com.Danliris.Service.Auth.Test.DataUtils
{
    public class RoleDataUtil : BaseDataUtil<Role, RoleViewModel, RoleService>
    {
        public RoleDataUtil() : base()
        {

        }
        public RoleDataUtil(RoleService service) : base(service)
        {
        }

        public override Role GetNewData()
        {
            return new Role()
            {
                Code = "code",
                Description = "desc",
                Name = "name",
                Permissions = new List<Permission2>()
                {
                    new Permission2()
                    {
                        //Division = "div",
                        //permission = 1,
                        //Unit = "unit",
                        //UnitCode = "unitcode",
                        //UnitId = 1,
                        Code = "div",
                        permission = 1,
                        Menu = "unit",
                        SubMenu = "unitcode",
                        MenuName = "name",
                    }
                }
            };
        }

        public Role GetDataInput()
        {
            return new Role()
            {
                Code = "code",
                Description = "desc",
                Name = "name",
                Permissions = new List<Permission2>()
                {
                    new Permission2()
                    {
                        Id =2,
                        Code = "div",
                        permission = 1,
                        Menu = "unit",
                        SubMenu = "unitcode",
                        MenuName = "name",
                        UId="UId"
                    }
                }
            };
        }

        public override RoleViewModel GetNewViewModel()
        {
            return new RoleViewModel()
            {
                code = "code",
                description = "desc",
                name = "name",
                permissions = new List<Permission2ViewModel>()
                {
                    new Permission2ViewModel()
                    {
                        permission = 1,
                        //menu = new MenuViewModel()
                        //{
                        Code = "div",
                        Menu = "unit",
                        SubMenu = "unitcode",
                        MenuName = "name",
                        
                        
                          
                        //}
                    }
                }
            };
        }
    }
}
