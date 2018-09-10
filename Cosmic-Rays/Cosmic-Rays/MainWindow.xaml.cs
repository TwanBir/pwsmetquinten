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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            LoadStations();
        }

        public class Station
        {
            [JsonProperty("number")]
            public string stationID { get; set; }

            [JsonProperty("name")]
            public string stationName { get; set; }

            //public bool activeStation { get; set; }
            //
            //public static readonly Station Default = new Station()
            //{
            //    activeStation = true;
            //};
        }

        public void LoadStations()
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
