using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public class DataSyncEventArgs<T> : EventArgs
    {
        public List<T> NewAppointments { get; set; }
    }
}
