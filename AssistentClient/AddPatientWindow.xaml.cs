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
    /// Interaction logic for AddPatientWindow.xaml
    /// </summary>
    public partial class AddPatientWindow : Window
    {
        private Regex _validName = new Regex(@"^[\p{Lu}][\p{Ll}]+");
        private Regex _validSSN = new Regex(@"[0-9]{9}");
        private IRESTService _apiService;
        

        public AddPatientWindow(IRESTService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            DataContext = this;
        }

        public async Task<bool> IsPatientDataValid()
        {
            bool result = true;
            if (!_validName.IsMatch(FirstNameTextBox.Text))
            {
                FirstNameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                FirstNameTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }

            if (!_validName.IsMatch(MiddleNameTextBox.Text) && !string.IsNullOrEmpty(MiddleNameTextBox.Text))
            {
                MiddleNameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                MiddleNameTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }

            if (!_validName.IsMatch(LastNameTextBox.Text))
            {
                LastNameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                LastNameTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            if (string.IsNullOrEmpty(AddressCityTextBox.Text))
            {
                AddressCityTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                AddressCityTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            if (string.IsNullOrEmpty(AddressZIPTextBox.Text))
            {
                AddressZIPTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                AddressZIPTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            if (string.IsNullOrEmpty(AddressStreetTextBox.Text))
            {
                AddressStreetTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                AddressStreetTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            if (string.IsNullOrEmpty(AddressNumberTextBox.Text))
            {
                AddressNumberTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                AddressNumberTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }

            if (!_validSSN.IsMatch(SocialSecurityIDTextBox.Text))
            {
                SocialSecurityIDTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                try
                {
                    var dummy = await _apiService.GetAllAsync<Patient>("patients", "socialSecurityId=" + SocialSecurityIDTextBox.Text);
                    Console.WriteLine("Patients count: " + dummy.Count);
                    if (dummy.Count > 0)
                    {
                        SocialSecurityIDTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                        TakenSSNLabel.Visibility = Visibility.Visible;
                        result = false;
                    }

                    else
                    {
                        SocialSecurityIDTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
                        TakenSSNLabel.Visibility = Visibility.Collapsed;
                    }

                }
                catch(Flurl.Http.FlurlHttpException ex)
                {
                    //Unsure tbh
                }
                
            }

            if (!DateOfBirthPicker.SelectedDate.HasValue)
            {
                result = false;
                DateOfBirthPicker.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                DateOfBirthPicker.BorderBrush = new SolidColorBrush(Colors.Green);
            }

            return result;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            
            if (await IsPatientDataValid())
            {
                Patient newPatient = new Patient
                {
                    FirstName = FirstNameTextBox.Text,
                    MiddleName = MiddleNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    SocialSecurityId = SocialSecurityIDTextBox.Text,
                    DateOfBirth = DateOfBirthPicker.SelectedDate.Value,
                    HomeAddress = new HomeAddress()
                    {
                        Country = "Hungary",
                        Province = "",
                        City = AddressCityTextBox.Text,
                        ZIP = AddressZIPTextBox.Text,
                        Street = AddressStreetTextBox.Text,
                        Address = AddressNumberTextBox.Text
                    }

                };
                try
                {
                    await _apiService.PostAsync("patients", newPatient);
                }
                catch(Flurl.Http.FlurlHttpException ex)
                {
                    ErrorLabel.Text = ex.Message;
                    Console.WriteLine(ex);
                }
                var result = MessageBox.Show("Patient successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                    this.Close();
            }
            else
            {
                ErrorLabel.Text = "Invalid input, please make sure you entered everything correctly!";
            }

        }

        private void SocialSecurityIDTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
