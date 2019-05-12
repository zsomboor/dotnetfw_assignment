using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DataServices
{
    /// <summary>
    /// A timer based synchronization service for periodically synchronizing a collection with a REST service.
    /// </summary>
    /// <typeparam name="T">The type of collection elements to be synchronized.</typeparam>
    public class PollingDataSyncService<T> : IDataSyncService<T> where T : ISyncableData
    {
        private Timer _timer;
        private List<T> _repository;
        private IRESTService _apiService;
        private string _apiResourceUri;
        private DataSyncDelegate<T> _dataSyncDelegate;

        /// <summary>
        /// Occurs when new elements are found.
        /// </summary>
        public event EventHandler<DataSyncEventArgs<T>> NewData;
        /// <summary>
        /// Occurs when already existing elements are changed or removed.
        /// </summary>
        public event EventHandler<DataSyncEventArgs<T>> AlteredData;

        /// <summary>
        /// Initializes a new PollingDataSyncService&lt;T&gt; instance
        /// </summary>
        /// <param name="interval">Polling rate in milliseconds.</param>
        /// <param name="apiService">The IRESTService instance to be used to make API calls.</param>
        /// <param name="dataSyncDelegate">Will be invoked as the synchronization method.</param>
        public PollingDataSyncService(int interval, IRESTService apiService, DataSyncDelegate<T> dataSyncDelegate = null)
        {
            _timer = new Timer(interval);
            _timer.Elapsed += SyncWithDatabase;
            _repository = new List<T>();
            _apiService = apiService;
            if (dataSyncDelegate != null)
                _dataSyncDelegate = dataSyncDelegate;
            else
                _dataSyncDelegate = new DataSyncDelegate<T>(DefaultDatabaseFetchFunc);
            _apiResourceUri = new StringBuilder(typeof(T).Name.ToLower()).Append("s").ToString();
        }

        /// <summary>
        /// Manually triggers a synchronization.
        /// </summary>
        public void ManualSync()
        {
            SyncWithDatabase(null, null);
        }
        /// <summary>
        /// Begins synchronization.
        /// </summary>
        public async Task Start()
        {
            if (_apiService == null)
                Console.WriteLine("That's dope man.");
            DateTime today = DateTime.Now.Subtract(DateTime.Now.TimeOfDay);
            DateTime tomorrow = today.AddDays(1);
            List<T> list;
            try
            {
                list = await _apiService.GetAllBetweenDatesAsync<T>(_apiResourceUri, today, tomorrow);
            }
            catch(Flurl.Http.FlurlHttpException ex)
            {
                throw;
            }
            Console.WriteLine("Number of initial elements: " + list.Count);
            if(list.Count > 0)
            {
                _repository.AddRange(list);
                NewData?.Invoke(this, new DataSyncEventArgs<T>() { ChangedElements = list });
            }
            //foreach (T item in _repository.Cast<T>())
            //{
            //    Console.WriteLine(item.GetSyncId());
            //}
            _timer.Start();
        }

        /// <summary>
        /// Ends synchronization.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        private async void SyncWithDatabase(object sender, EventArgs e)
        {
            var list = await _dataSyncDelegate(_apiService, _apiResourceUri, _repository.Count > 0  ? _repository.Max(x => x.GetSyncId()) : -1);
            //Console.WriteLine("Sync Call made");
            if (list.Count > 0)
            {
                Console.WriteLine("More than 0 ({0}) new elements retrieved!", list.Count);
                _repository.AddRange(list);
                NewData?.Invoke(this, new DataSyncEventArgs<T>() { ChangedElements = list });
            }

            //If there are no elements, nothing can change
            if (_repository.Count == 0)
                return;


            var alteredList = await ItemsAlteredFetchFunc(_apiService, _apiResourceUri, _repository.Min(x => x.GetSyncDate()));

            if(alteredList.Count > 0)
            {
                //This is needed because the minimum date might be lower than the first processed element's date due to appointments taking priority.
                //Probably should be moved to an upper layer as this behaviour is CheckIn specific and this class is otherwise trying to be as generic as possible
                List<T> notifyWithList = new List<T>();
                foreach (var item in alteredList)
                {
                    //Console.WriteLine(item.GetSyncId() + ": " + item.GetSyncDate());
                    T removable = _repository.SingleOrDefault(x => x.GetSyncId() == item.GetSyncId());
                    if (removable != null)
                    {
                        _repository.Remove(removable);
                        notifyWithList.Add(item);
                    }
                }
                if(notifyWithList.Count > 0)
                    AlteredData?.Invoke(this, new DataSyncEventArgs<T>() { ChangedElements = notifyWithList });
            }
        }

        /// <summary>
        /// The default function for API calls if no delegate is passed in the constructor.
        /// </summary>
        /// <param name="apiService">The IRESTService instance to be used to make the API call.</param>
        /// <param name="resourceUri">The URL path beyond the base URL.</param>
        /// <param name="syncBy">The value to sync by, elements with greater Id will be returned.</param>
        /// <returns>List of new T elements, may be empty.</returns>
        private async Task<List<T>> DefaultDatabaseFetchFunc(IRESTService apiService, string resourceUri, int syncBy)
        {
            //Console.WriteLine("Sync by id: {0}", syncBy);
            DateTime today = DateTime.Now.Subtract(DateTime.Now.TimeOfDay);
            DateTime tomorrow = today.AddDays(1);
            return await apiService.GetAllBetweenDatesAsync<T>(resourceUri, today, tomorrow, syncBy > -1 ? syncBy : (int?)null);
            //return await _apiService.GetAllAfterDateAsync<T>(resourceUri, new DateTime(), syncBy);
        }

        private async Task<List<T>> ItemsAlteredFetchFunc(IRESTService apiService, string resourceUri, DateTime syncByDate)
        {
            //Console.WriteLine("Syncing by date: {0}", syncByDate);
            DateTime today = DateTime.Now.Subtract(DateTime.Now.TimeOfDay);
            DateTime tomorrow = today.AddDays(1);
            return await apiService.GetAllBetweenDatesAsync<T>(resourceUri, syncByDate, tomorrow, null, true);
            //return await _apiService.GetAllAfterDateAsync<T>(resourceUri, new DateTime(), syncBy);
        }

    }
}
