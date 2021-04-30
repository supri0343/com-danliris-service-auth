using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Models
{
    public class AccountRole : StandardEntity
    {
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public string RoleUId { get; set; }

        public string UId { get; set; }

    }
}
