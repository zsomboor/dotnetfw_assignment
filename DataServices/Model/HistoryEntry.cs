using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Model
{
    public class HistoryEntry
    {
        public long Id { get; set; }
        public long CheckInId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
