﻿using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Models
{
    public class Account : StandardEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLocked { get; set; }
        public virtual AccountProfile AccountProfile { get; set; }
        public virtual ICollection<AccountRole> AccountRoles { get; set; }

        public string UId { get; set; }

    }
}
