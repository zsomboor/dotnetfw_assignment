using DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Model
{
    public class CheckIn : ISyncableData
    {
        public long Id { get; set; }
        public DateTime? Arrival { get; set; }
        public DateTime? AppointedTo { get; set; }
        public bool IsAppointment { get; set; }
        public string Description { get; set; }
        public Patient Patient { get; set; }

        public DateTime GetSyncDate()
        {
            return AppointedTo.Value;
        }

        public int GetSyncId()
        {
            return (int)Id;
        }
    }
}
