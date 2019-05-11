using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    [Owned]
    public class HomeAddress
    {
        public string Country { get; set; }
        public string Province { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
    }
}