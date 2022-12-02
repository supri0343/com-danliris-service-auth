using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Models
{
    public class Permission2 : StandardEntity
    {
        //public int MenuId { get; set; }
        public string Code { get; set; }
        public string Menu { get; set; }
        public string SubMenu { get; set; }
        public string MenuName { get; set; }
        public int permission { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public string UId { get; set; }
    }
}
