using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GridViewNumericList.Models
{
    public class GridMvcFilter
    {
        public const int DefaultPagesize = 10;
        public string Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string GridOrd { get; set; }
        public string CurrentFilter { get; set; }
        public string GridFilter { get; set; }
        public int? GridPage { get; set; }
        public int? GridPageSize { get; set; }
    }
}