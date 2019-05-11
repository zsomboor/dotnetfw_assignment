using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public delegate Task<List<T>> DataSyncDelegate<T>(IRESTService apiService, string resourceUri, int syncBy);
}
