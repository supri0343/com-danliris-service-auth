using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.BusinessLogic.Services
{
    public class AccountMonitoringViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Menu { get; set; }
        public string SubMenu { get; set; }
        public string MenuItems { get; set; }
        public int Count { get; set; }

    }
}
