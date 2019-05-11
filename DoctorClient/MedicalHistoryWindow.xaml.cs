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
    /// Interaction logic for MedicalHistoryWindow.xaml
    /// </summary>
    public partial class MedicalHistoryWindow : Window
    {
        public List<HistoryEntry> Entries { get; set; }

        public MedicalHistoryWindow(List<HistoryEntry> entries)
        {
            Entries = entries;
            DataContext = this;
            InitializeComponent();
        }
    }
}
