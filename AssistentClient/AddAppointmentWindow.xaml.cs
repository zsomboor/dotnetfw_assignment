
using DataServices;
using DataServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AssistantClient
{
    /// <summary>
    /// Interaction logic for AddAppointmentWindow.xaml
    /// </summary>
    public partial class AddAppointmentWindow : Window
    {
        private IRESTService _apiService;

        private List<TimeSpan> appointmentTimes;

        private Patient selectedPatient = null;

        public bool IsDateSelected
        {
            get { return (bool)GetValue(IsDateSelectedProperty); }
            set { SetValue(IsDateSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDateSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDateSelectedProperty =
            DependencyProperty.Register("IsDateSelected", typeof(bool), typeof(AddAppointmentWindow));


        private void SetupDebugPatientList()
        {
            List<Patient> patients = new List<Patient>()
            {
                new Patient()
                {
                    FirstName = "Firstname",
                    LastName = "Lastname",
                    DateOfBirth = new DateTime(1997, 07, 13),
                    SocialSecurityId = "123654879"
                },
                new Patient()
                {
                    FirstName = "Firstname",
                    MiddleName = "Middlename",
                    LastName = "Lastname",
                    DateOfBirth = new DateTime(1997, 07, 13),
                    SocialSecurityId = "123654879"
                },
                new Patient()
                {
                    FirstName = "Firstname",
                    MiddleName = "Middlename",
                    LastName = "Lastname",
                    DateOfBirth = new DateTime(1997, 07, 13),
                    SocialSecurityId = "123654879"
                },
                new Patient()
                {
                    FirstName = "Firstname",
                    MiddleName = "Middlename",
                    LastName = "Lastname",
                    DateOfBirth = new DateTime(1997, 07, 13),
                    SocialSecurityId = "123654879"
                },
            };

            SearchResultPatientsListBox.ItemsSource = patients;
        }

        public bool IsDateValid(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;
            return true;
        }

        public bool IsTimeOfDayValid(DateTime time)
        {
            Console.WriteLine(time.TimeOfDay);
            if (time.TimeOfDay.CompareTo(TimeSpan.FromMinutes(8 * 60)) < 0 || time.TimeOfDay.CompareTo(TimeSpan.FromMinutes(17 * 60 - 10)) > 0)
                return false;
            return true;
        }

        public async void FetchAppointmentsForDay(DateTime date)
        {
            try
            {
                List<CheckIn> checkins = await _apiService.GetAllBetweenDatesAsync<CheckIn>("checkins", date, date.AddDays(1));
                appointmentTimes = checkins.Where(x => x.IsAppointment == true).Select(x => x.AppointedTo.Value.TimeOfDay).ToList();
            }
            catch(Flurl.Http.FlurlHttpException ex)
            {
                appointmentTimes = new List<TimeSpan>(){
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 20),
                    TimeSpan.FromMinutes(3*60 + 30),
                    TimeSpan.FromMinutes(8*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(10*60 + 50),
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 10),
                    TimeSpan.FromMinutes(3*60 + 10),

                };
                Console.WriteLine("Yeeted");
            }
            AppointmentsForDateItemsControl.ItemsSource = appointmentTimes;
        }

        private async void FetchMatchingPatients()
        {
            List<string> queryParamsList = new List<string>();
            if (!string.IsNullOrEmpty(FirstNameTextBox.Text))
                queryParamsList.Add("firstName=" + FirstNameTextBox.Text);
            if (!string.IsNullOrEmpty(MiddleNameTextBox.Text))
                queryParamsList.Add("middleName=" + MiddleNameTextBox.Text);
            if (!string.IsNullOrEmpty(LastNameTextBox.Text))
                queryParamsList.Add("lastName=" + LastNameTextBox.Text);
            if (!string.IsNullOrEmpty(SocialSecurityIDTextBox.Text))
                queryParamsList.Add("socialSecurityId=" + SocialSecurityIDTextBox.Text);

            try
            {
                SearchResultPatientsListBox.ItemsSource = await _apiService.GetAllAsync<Patient>("patients", queryParamsList.ToArray());
            }
            catch(Flurl.Http.FlurlHttpException ex)
            {
                MessageBox.Show("Could not retrieve patients: " + ex.Message ,"Error Retrieving Patients", MessageBoxButton.OK, MessageBoxImage.Warning);
                //SetupDebugPatientList();
            }
        }

        public AddAppointmentWindow(IRESTService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            DataContext = this;
            IsDateSelected = false;
            AppointmentsForDateItemsControl.ItemsSource = appointmentTimes;
        }


        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePicker.SelectedDate.HasValue && IsDateValid(DatePicker.SelectedDate.Value))
            {
                IsDateSelected = true;
                FetchAppointmentsForDay(DatePicker.SelectedDate.Value);
            }
            else
            {
                IsDateSelected = false;
                appointmentTimes = null;
                AppointmentsForDateItemsControl.ItemsSource = null;
            }
        }

        private void TimePicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Console.WriteLine(TimePicker.Value);
            Console.WriteLine(IsTimeOfDayValid(TimePicker.Value.Value));
        }

        private void SocialSecurityIDTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FetchMatchingPatients();
            selectedPatient = null;
        }

        private void SearchResultPatientsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPatient = SearchResultPatientsListBox.SelectedItem as Patient;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(!isAppointmentCheckBox.IsChecked.Value && selectedPatient != null && !string.IsNullOrEmpty(DescriptionTextBox.Text)
                || isAppointmentCheckBox.IsChecked.Value && selectedPatient != null && !string.IsNullOrEmpty(DescriptionTextBox.Text) && DatePicker.SelectedDate.HasValue && IsDateValid(DatePicker.SelectedDate.Value) && TimePicker.Value.HasValue && IsTimeOfDayValid(TimePicker.Value.Value))
            {
                CheckIn checkin = new CheckIn()
                {
                    AppointedTo = isAppointmentCheckBox.IsChecked.Value ? DatePicker.SelectedDate.Value.Add(TimePicker.Value.Value.TimeOfDay) : (DateTime?)null,
                    Description = DescriptionTextBox.Text,
                    IsAppointment = isAppointmentCheckBox.IsChecked.Value
                };

                try
                {
                    await _apiService.PostAsync("patients/" + selectedPatient.Id + "/checkins", checkin);
                    var result = MessageBox.Show("Appointment successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (result == MessageBoxResult.OK)
                        this.Close();
                }
                catch(Flurl.Http.FlurlHttpException ex)
                {
                    ErrorTextBox.Text = ex.Message;
                }

            }
            else
            {
                ErrorTextBox.Text = "Invalid input, please make sure you entered everything correctly!";
            }

        }
    }
}
