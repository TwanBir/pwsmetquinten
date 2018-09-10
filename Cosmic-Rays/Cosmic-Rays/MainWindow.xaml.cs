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
            LoadStations();
        }

        private void stationDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateActiveStations();
        }

        public class Station
        {
            [JsonProperty("number")]
            public string stationID { get; set; }

            [JsonProperty("name")]
            public string stationName { get; set; }

            public bool activeStation { get; set; }
        }

        public void LoadStations()
        {
            using (var webClient = new System.Net.WebClient())
            {
                //gets json file
                string json = webClient.DownloadString("http://data.hisparc.nl/api/stations/");
                //converts .json to list of objects of class Station
                List<Station> stations = JsonConvert.DeserializeObject<List<Station>>(json);
                //loops to objects to add them to the datagrid
                for (int i = 0; i < stations.Count; i++)
                {
                    stationGrid.Items.Add(stations[i]);
                }
            }
        }

        public void UpdateActiveStations()
        {
            using (var webClient = new System.Net.WebClient())
            {
                stationGrid.Items.Clear();
                string jsonall = webClient.DownloadString("http://data.hisparc.nl/api/stations/data/");
                List<Station> allStations = JsonConvert.DeserializeObject<List<Station>>(jsonall);
                if (stationDateFilter.SelectedDate < DateTime.Now)
                {
                    DateTime dateTimeFilter = stationDateFilter.SelectedDate ?? DateTime.Now;
                    string json = webClient.DownloadString($"http://data.hisparc.nl/api/stations/data/" + dateTimeFilter.ToString("yyyy") + "/" + dateTimeFilter.ToString("MM") + "/" + dateTimeFilter.ToString("dd") + "/");
                    List<Station> stationsActive = JsonConvert.DeserializeObject<List<Station>>(json);

                    for (int x = 0; x < allStations.Count; x++)
                    {
                        for (int i = 0; i < stationsActive.Count; i++)
                        {
                            if (stationsActive[i].stationName == allStations[x].stationName)
                            {
                                allStations[x].activeStation = true;
                                break;
                            }
                            else
                            {
                                allStations[x].activeStation = false;
                            }
                        }
                        stationGrid.Items.Add(allStations[x]);
                    }

                }
                else
                {
                    for (int x = 0; x < allStations.Count; x++)
                    {
                        stationGrid.Items.Add(allStations[x]);
                    }
                }    

            }
        }
        

        public void LoadStations_old()
        {
            using (var webClient = new System.Net.WebClient())
            {
                stationGrid.Items.Clear();
                string json = null;
                if (stationDateFilter.SelectedDate > DateTime.Now) { return; }
                else if (stationDateFilter.Text == "")
                { 
                    json = webClient.DownloadString("http://data.hisparc.nl/api/stations/data/");
                }
                    else
                {

                    DateTime dateTimeFilter = stationDateFilter.SelectedDate ?? DateTime.Now;
                    textBox.Text = dateTimeFilter.ToString("yyyy") + "/" + dateTimeFilter.ToString("MM") + "/" + dateTimeFilter.ToString("dd");
                    //json = webClient.DownloadString("http://data.hisparc.nl/api/stations/data/");
                    json = webClient.DownloadString($"http://data.hisparc.nl/api/stations/data/" + dateTimeFilter.ToString("yyyy") + "/" + dateTimeFilter.ToString("MM") + "/" + dateTimeFilter.ToString("dd") + "/");
                }
                //var json = webClient.DownloadString("http://data.hisparc.nl/api/stations/");
                //JArray jArray = JArray.Parse(json);
                List<Station> stations = JsonConvert.DeserializeObject<List<Station>>(json);
                //foreach (JObject item in jArray.Children())
                //{
                //    string name = (string)item.SelectToken("name");
                //    string id = (string)item.SelectToken("number");
                //
                //}
                //textBox.Text = stations[1].stationName;
                for (int i = 0; i < stations.Count; i++)
                {
                    stationGrid.Items.Add(stations[i]);
                }

            }
        }
    }
}
