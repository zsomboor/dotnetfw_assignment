using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    /// <summary>
    /// A simple interface for synchronizing a collection with another source.
    /// </summary>
    /// <typeparam name="T">The type of collection elements to be synchronized.</typeparam>
    public interface IDataSyncService<T> where T : ISyncableData
    {
        /// <summary>
        /// Occurs when new elements are found.
        /// </summary>
        event EventHandler<DataSyncEventArgs<T>> NewData;
        /// <summary>
        /// Occurs when already existing elements have been altered.
        /// </summary>
        event EventHandler<DataSyncEventArgs<T>> AlteredData;
        /// <summary>
        /// Begins synchronization.
        /// </summary>
        Task Start();
        /// <summary>
        /// Ends synchronization.
        /// </summary>
        void Stop();
        /// <summary>
        /// Manually triggers a synchronization.
        /// </summary>
        void ManualSync();

    }
}
