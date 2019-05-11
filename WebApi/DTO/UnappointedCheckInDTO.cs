using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.DTO
{
    public class UnappointedCheckInDTO
    {
        public DateTime Arrival { get; set; }
        public PatientBaseDTO Patient { get; set; }
    }
}