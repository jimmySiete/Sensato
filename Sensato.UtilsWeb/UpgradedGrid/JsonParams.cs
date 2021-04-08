using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlugginCreation.Models
{
    public class JsonParams
    {
        public int filter { get; set; }
        public int actualPage { get; set; }
        public bool isAsc { get; set; }
        public string orderBy { get; set; }
        public bool literacy { get; set; }
    }
}