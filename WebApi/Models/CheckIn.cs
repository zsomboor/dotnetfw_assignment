using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class CheckIn
    {
        public long Id { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime AppointedTo { get; set; }
        public bool IsAppointment { get; set; }
        public bool IsDone { get; set; }
        public virtual Patient Patient { get; set; }
        public string Description { get; set; }
    }
}