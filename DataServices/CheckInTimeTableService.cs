using DataServices.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DataServices
{
    ///<summary>
    /// Manages a List of Check-Ins:
    /// <para/>      - Internal syncing with the backend every 5 seconds by default
    /// <para/>      - Monitoring appointments and updating the order every 5 seconds by default, moving an appointment to the top once it matches or is before the current time
    ///</summary>
    ///<remarks>
    /// Internally uses an IDataSyncService to synchronize with the backend. 
    ///</remarks>
    public class CheckInTimeTableService
    {
        //Could not directly use an ObservableCollection as it does not allow off-thread altering
        /// <summary>
        /// Contains the currently available CheckIns in the proper order.
        /// </summary>
        public List<CheckIn> CheckIns { get; set; }
        private IRESTService _apiService;
        private IDataSyncService<CheckIn> _dataSyncService;
        private bool _isFirstSync;
        /// <summary>
        /// Occurs when the CheckIns List is altered.
        /// </summary>
        public event EventHandler Updated;
        private Timer _timer;

        public CheckInTimeTableService(IRESTService apiService)
        {
            _isFirstSync = true;
            CheckIns = new List<CheckIn>();
            _apiService = apiService;
            _dataSyncService = new PollingDataSyncService<CheckIn>(5000, _apiService);
            _dataSyncService.NewData += OnNewElementsFound;
            _dataSyncService.AlteredData += OnAlteredElementsFound;
            _timer = new Timer(5000);
            _timer.Elapsed += OnAppointmentSortRequest;
        }

        /// <summary>
        /// Initiates periodic sync with the backend and appointment order monitoring.
        /// </summary>
        public async Task Start()
        {
            try
            {
                await _dataSyncService.Start();
            }
            catch(Flurl.Http.FlurlHttpException ex)
            {
                Console.WriteLine("Could not start synchronization: \n" + ex.Message);
                throw;
            }
            _timer.Start();
        }

        /// <summary>
        /// Stops database synchronization and order monitoring.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            _dataSyncService.Stop();
        }

        private void OnNewElementsFound(object sender, DataSyncEventArgs<CheckIn> e)
        {
            List<CheckIn> newElements = e.ChangedElements.OrderBy(x => x.AppointedTo).ToList();
            for(int i = 0; i < newElements.Count; i++)
            {
                if (newElements[i].IsAppointment && newElements[i].AppointedTo.Value.CompareTo(DateTime.Now) <= 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (!newElements[j].IsAppointment)
                        {
                            CheckIn temp = newElements[i];
                            newElements.RemoveAt(i);
                            newElements.Insert(j, temp);
                            break;
                        }
                    }
                }

                if (i < newElements.Count - 1 && !newElements[i].IsAppointment && newElements[i+1].IsAppointment && newElements[i+1].AppointedTo.Value.CompareTo(newElements[i].AppointedTo.Value) == 0)
                {
                    var temp = newElements[i];
                    newElements[i] = newElements[i + 1];
                    newElements[i + 1] = temp;
                }
            }

            foreach (var item in newElements)
            {
                //Puts newly synchronized checkins that are earlier than the current last one in their right place.
                if (CheckIns.Count > 1 && CheckIns.Last().AppointedTo.Value.CompareTo(item.AppointedTo.Value) > 0)
                {
                    int targetIndex = CheckIns.Count - 1;
                    while(targetIndex > 1 && CheckIns[targetIndex - 1].AppointedTo.Value.CompareTo(item.AppointedTo.Value) > 0)
                    {
                        targetIndex--;
                    }
                    CheckIns.Insert(targetIndex, item);

                }
                else
                    CheckIns.Add(item);
            }


            Updated.Invoke(this, new EventArgs());
        }

        private void OnAppointmentSortRequest(object sender, EventArgs e)
        {
            bool hasChanged = false;
            for(int i = 0; i < CheckIns.Count; i++)
            {
                if (CheckIns[i].IsAppointment && CheckIns[i].AppointedTo.Value.CompareTo(DateTime.Now) <= 0)
                {
                    for (int j = 1; j < i; j++)
                    {
                        if (!CheckIns[j].IsAppointment)
                        {
                            CheckIn temp = CheckIns[i];
                            CheckIns.RemoveAt(i);
                            CheckIns.Insert(j, temp);
                            hasChanged = true;
                            break;
                        }
                    }  
                }
            }
            if(hasChanged)
                Updated.Invoke(this, new EventArgs());
        }

        private void OnAlteredElementsFound(object sender, DataSyncEventArgs<CheckIn> e)
        {
            if(e.ChangedElements.Count > 0)
            {
                foreach (var item in e.ChangedElements)
                {
                    //Console.WriteLine("Yay, this works!?");
                    //Console.WriteLine(item.Description);
                    Console.WriteLine("Removing Element: " + item.Id);
                    CheckIns.Remove(CheckIns.Single(x => x.Id == item.Id));
                }
                Updated.Invoke(this, new EventArgs());
            }         
        }

    }
}
