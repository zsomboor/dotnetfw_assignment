using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Model
{
    public class Patient
    {
        public long Id { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public HomeAddress HomeAddress { get; set; }
        public string SocialSecurityId { get; set; }
    }
}
