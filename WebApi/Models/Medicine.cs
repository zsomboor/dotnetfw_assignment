using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Medicine
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
    }
}