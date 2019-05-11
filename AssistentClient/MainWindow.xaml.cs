using DataServices;
using DataServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AssistantClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IRESTService _apiService;
        List<CheckIn> checkIns;

        private CheckInTimeTableService _timeTableService;

        //async void LoadNetworkData()
        //{
        //    try
        //    {
        //        checkIns = await _apiService.GetAllAsync<CheckIn>("checkins");
        //        LivePatientsListBox.ItemsSource = checkIns;
        //    }
        //    catch(Exception ex)
        //    {
        //        CheckIn a = new CheckIn();
        //        a.Id = 8323759;
        //        a.IsAppointment = true;
        //        a.AppointedTo = DateTime.Now;

        //        CheckIn b = new CheckIn();
        //        b.Id = 892823759;
        //        b.IsAppointment = true;
        //        b.AppointedTo = DateTime.Now.AddHours(1.5);


        //        CheckIn c = new CheckIn();
        //        c.Id = 1823759;
        //        c.IsAppointment = false;
        //        c.AppointedTo = DateTime.Now;
        //        checkIns = new List<CheckIn>();
        //        checkIns.Add(a);
        //        checkIns.Add(b);
        //        checkIns.Add(c);
        //        checkIns.Add(b);
        //        checkIns.Add(c);
        //        checkIns.Add(b);
        //        checkIns.Add(c);
        //        checkIns.Add(b);
        //        checkIns.Add(c);
        //        checkIns.Add(b);
        //        checkIns.Add(c);
        //        checkIns.Add(b);
        //        checkIns.Add(c);
        //        LivePatientsListBox.ItemsSource = checkIns;
        //    }

        //}

        public void OnTimeTableUpdated(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                LivePatientsListBox.ItemsSource = _timeTableService.CheckIns;
                LivePatientsListBox.Items.Refresh();
            });      
        }

        public async void InitializeTimeTable()
        {
            _timeTableService = new CheckInTimeTableService(_apiService);
            _timeTableService.Updated += OnTimeTableUpdated;
            bool retry;
            do
            {
                try
                {
                    await _timeTableService.Start();
                    retry = false;
                }
                catch (Flurl.Http.FlurlHttpException ex)
                {
                    var result = MessageBox.Show("Could not connect to server: " + ex.Message + Environment.NewLine + "Click Yes to retry, or No to close the application.", "Timetable Synchronization Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    retry = result == MessageBoxResult.Yes;
                    if (result == MessageBoxResult.No)
                        Application.Current.Shutdown();
                }
            } while (retry);
        }

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new RESTService("http://localhost:60879/api");

            InitializeTimeTable();
            //LoadNetworkData();

        }

        private void AddPatientButton_Click(object sender, RoutedEventArgs e)
        {
            Window addPatientWindow = new AddPatientWindow(_apiService);
            addPatientWindow.ShowDialog();
        }

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            Window addAppointmentWindow = new AddAppointmentWindow(_apiService);
            addAppointmentWindow.ShowDialog();
        }
    }
}
