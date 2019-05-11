using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class HistoryEntry
    {
        public long Id { get; set; }
        public long CheckInId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        //public List<Medicine> Prescription { get; set; }
    }
}