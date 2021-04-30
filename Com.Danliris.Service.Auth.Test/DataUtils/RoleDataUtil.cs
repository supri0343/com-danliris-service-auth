﻿using Com.Danliris.Service.Auth.Lib.BusinessLogic.Services;
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
                Permissions = new List<Permission>()
                {
                    new Permission()
                    {
                        Division = "div",
                        permission = 1,
                        Unit = "unit",
                        UnitCode = "unitcode",
                        UnitId = 1,
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
                Permissions = new List<Permission>()
                {
                    new Permission()
                    {
                        Id =2,
                        Division = "div",
                        permission = 1,
                        Unit = "unit",
                        UnitCode = "unitcode",
                        UnitId = 1,
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
                permissions = new List<PermissionViewModel>()
                {
                    new PermissionViewModel()
                    {
                        permission = 1,
                        unit = new UnitViewModel()
                        {
                            Id = 1,
                            Name = "name",
                            Code = "code",
                            Division = new DivisionViewModel()
                            {
                                Name = "divName"
                            }
                        }
                    }
                }
            };
        }
    }
}
