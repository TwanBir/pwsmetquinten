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
// add observable collections
using System.Collections.ObjectModel;
// add json usage
using Newtonsoft.Json;
// add url encode
using System.Net.Http;


namespace Cosmic_Rays.tabs
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var base_url = "http://data.hisparc.nl/data/network/coincidences/?{0}";
            var startDate = BeginDate.SelectedDate;
            var endDate = EndDate.SelectedDate;
            var n = StationCount;

        }
    }
}
