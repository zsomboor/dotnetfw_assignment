using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.DTO
{
    public class AppointmentDTO
    {
        [Required]
        public DateTime AppointedTo { get; set; }
    }
}