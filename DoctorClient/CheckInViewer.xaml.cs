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
using System.Windows.Shapes;

namespace DoctorClient
{
    /// <summary>
    /// Interaction logic for CheckInViewer.xaml
    /// </summary>
    public partial class CheckInViewer : Window
    {
        public CheckIn CheckIn { get; set; }
        public bool CanFinish { get; set; }
        private IRESTService _apiService;

        public CheckInViewer(IRESTService apiService, CheckIn checkIn, bool canFinish = false)
        {
            DataContext = this;
            CanFinish = canFinish;
            CheckIn = checkIn;
            _apiService = apiService;
            InitializeComponent();
        }

        private async void OpenMedicalHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Implement this function and window.
            try
            {
                List<HistoryEntry> entries = await _apiService.GetAllAsync<HistoryEntry>("patients/" + CheckIn.Patient.Id + "/medical-history");
                new MedicalHistoryWindow(entries).Show();
            }
            catch (Flurl.Http.FlurlHttpException ex)
            {
                Console.WriteLine(ex.GetResponseStringAsync());
            }

        }

        private async void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(DescriptionTextBox.Text) && MessageBox.Show("Are you sure you want to finish? Once you proceed this cannot be undone!", "Finishing Examination", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                HistoryEntry historyEntry = new HistoryEntry()
                {
                    Description = DescriptionTextBox.Text
                };
                try
                {
                    await _apiService.PostAsync<HistoryEntry>("checkins/" + CheckIn.Id + "/is-done", historyEntry);
                    var result = MessageBox.Show("Save Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (result == MessageBoxResult.OK)
                        this.Close();
                }
                catch(Flurl.Http.FlurlHttpException ex)
                {
                    ErrorTextBox.Text =  ex.Message;
                    Console.WriteLine(await ex.GetResponseStringAsync());
                }
            }
            else if (string.IsNullOrEmpty(DescriptionTextBox.Text))
            {
                ErrorTextBox.Text = "A final note is required before finishing!";
            }
        }
    }
}
