using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.DTO
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PatientBaseDTO
    {
        public long Id { get; set; }
        public DateTime AddedAt { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("[0-9]{9}")]
        public string SocialSecurityId { get; set; }
        [Required]
        public HomeAddress HomeAddress { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

    }
}