using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.DTO
{
    public class HistoryEntryDTO
    {
        public long Id { get; set; }
        public long CheckInId { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string Description { get; set; }
    }
}