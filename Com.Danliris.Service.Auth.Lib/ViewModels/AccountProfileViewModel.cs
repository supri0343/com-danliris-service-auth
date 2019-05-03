using Com.Danliris.Service.Auth.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.ViewModels
{
    public class AccountProfileViewModel 
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string gender { get; set; }
        public DateTime? dob { get; set; }
        public string email { get; set; }
        
    }
}
