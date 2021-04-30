﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Utilities.BaseClass
{
    public class BaseOldViewModel
    {
        public int _id { get; set; }
        public bool _deleted { get; set; }
        public bool _active { get; set; }
        public DateTime _createdDate { get; set; }
        public string _createdBy { get; set; }
        public string _createAgent { get; set; }
        public DateTime _updatedDate { get; set; }
        public string _updatedBy { get; set; }
        public string _updateAgent { get; set; }
    }
}
