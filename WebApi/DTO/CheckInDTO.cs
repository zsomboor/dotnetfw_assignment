using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.DTO
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CheckInDTO
    {
        public long Id { get; set; }
        public DateTime? Arrival { get; set; }
        public DateTime? AppointedTo { get; set; }
        [Required]
        public bool IsAppointment { get; set; }
        [Required]
        public string Description { get; set; }
        public PatientBaseDTO Patient { get; set; }
    }
}