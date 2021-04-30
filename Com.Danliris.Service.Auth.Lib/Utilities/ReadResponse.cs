﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Utilities
{
    public class ReadResponse<TModel>
    {
        public List<TModel> Data { get; set; }
        public int Count { get; set; }
        public Dictionary<string, string> Order { get; set; }
        public List<string> Selected { get; set; }
        public ReadResponse(List<TModel> data, int count, Dictionary<string, string> order, List<string> selected)
        {
            this.Data = data;
            this.Count = count;
            this.Order = order;
            this.Selected = selected;
        }
    }
}
