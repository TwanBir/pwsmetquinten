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
using Newtonsoft.Json.Linq;

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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString("http://data.hisparc.nl/api/stations/");
                JArray jArray = JArray.Parse(json);
                foreach (JObject item in jArray.Children())
                {
                    string name = (string)item.SelectToken("name");
                    string id = (string)item.SelectToken("number");
                }
                


            }
        }
    }
}
