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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Data;
//for observableclass
using System.Collections.ObjectModel;

namespace Cosmic_Rays
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            stationGrid.ItemsSource = new StationList().GetStations();
        }

        private void stationDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateActiveStations();
        }

        public class StationList
        {
            public List<Station> GetStations()
            {
                return LoadSations();
            }

            private List<Station> LoadSations()
            {
                using (var webClient = new System.Net.WebClient())
                {
                    // gets json file
                    string json = webClient.DownloadString("http://data.hisparc.nl/api/stations/");
                    //converts .json to list of objects of class Station
                    List<Station> list = JsonConvert.DeserializeObject<List<Station>>(json);
                    // loops to objects to add them to the datagrid
                    return list;
                }
            }
        }

        public class Station
        {
            [JsonProperty("number")]
            public string stationID { get; set; }

            [JsonProperty("name")]
            public string stationName { get; set; }

            public bool activeStation { get; set; }
        }


        public void UpdateActiveStations()
        {
            using (var webClient = new System.Net.WebClient())
            {
                // checks of set date is earlier then today
                if (stationDateFilter.SelectedDate < DateTime.Now)
                {
                    // makes it so that the variable dateTimeFilter always has a value
                    DateTime dateTimeFilter = stationDateFilter.SelectedDate ?? DateTime.Now;
                    // gets raw json data from the server
                    string json = webClient.DownloadString($"http://data.hisparc.nl/api/stations/data/" + dateTimeFilter.ToString("yyyy") + "/" + dateTimeFilter.ToString("MM") + "/" + dateTimeFilter.ToString("dd") + "/");
                    // converts json data to .net list
                    List<Station> stationsActive = JsonConvert.DeserializeObject<List<Station>>(json);
                    // loops thru all items in datagrid
                    foreach (var item in stationGrid.Items.OfType<Station>())
                    {
                        // loops thru all items in activestation list
                        for (int i = 0; i < stationsActive.Count; i++)
                        {
                            // if 2 names mach station is marked active and function will break
                            if (stationsActive[i].stationName == item.stationName)
                            {
                                item.activeStation = true;
                                break;
                            }
                        }
                    }
                }
                // if the date set by user is in the future (must revise when time travel is within reach)
                else
                {
                    // sets the flags from all rows to false
                    foreach (var item in stationGrid.Items.OfType<Station>())
                    {
                        item.activeStation = false;
                    }
                }
                stationGrid.Items.Refresh();
            }
        }
    }
}
