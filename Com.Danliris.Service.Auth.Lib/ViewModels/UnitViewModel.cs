using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.ViewModels
{
    public class UnitViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public DivisionViewModel Division { get; set; }
    }
}
