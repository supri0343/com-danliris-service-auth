﻿using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Models
{
    public class Permission : StandardEntity
    {
        public int UnitId { get; set; }
        public string UnitCode { get; set; }
        public string Unit { get; set; }
        public string Division { get; set; }
        public int permission { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public string UId { get; set; }

    }
}
