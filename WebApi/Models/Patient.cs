using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Patient
    {
        public long Id { get; set; }
        public DateTime AddedAt { get; set;  }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set;  }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityId { get; set; }
        public HomeAddress HomeAddress { get; set; }
        public virtual List<CheckIn> CheckIns { get; set; }
        public virtual List<HistoryEntry> MedicalHistory { get; set; }

        public Patient()
        {

        }

        public Patient(string name)
        {
            FirstName = name;
            AddedAt = DateTime.UtcNow;
        }

        public Patient(string name, DateTime time)
        {
            FirstName = name;
            AddedAt = time;
        }
    }
}