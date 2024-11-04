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
using System.Net.Http;
using Newtonsoft.Json;

namespace App_Météo_LRD
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadWeather();
        }

        public async Task<Root> GetWeather()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.openweathermap.org/data/2.5/weather?q=annecy,fr&appid=c21a75b667d6f7abb81f118dcf8d4611&units=metric");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Root weatherResponse = JsonConvert.DeserializeObject<Root>(content);
                return weatherResponse;
            }
            else
            {
                return null;
            }
        }

        private async void LoadWeather()
        {
            Root weatherData = await GetWeather();
            if (weatherData != null)
            {
                Dispatcher.Invoke(() =>
                {
                    TemperatureTextBlock.Text = $"Température: {weatherData.main.temp}°C";
                    FeelsLikeTextBlock.Text = $"Ressenti: {weatherData.main.feels_like}°C";
                    TempMinTextBlock.Text = $"Température Min: {weatherData.main.temp_min}°C";
                    TempMaxTextBlock.Text = $"Température Max: {weatherData.main.temp_max}°C";
                    HumidityTextBlock.Text = $"Humidité: {weatherData.main.humidity}%";
                    CloudTextBlock.Text = GetCloudDescription(weatherData.clouds.all);
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    TemperatureTextBlock.Text = "Erreur de récupération des données";
                });
            }
        }
        private string GetCloudDescription(int cloudiness)
        {
            if (cloudiness == 0)
            {
                return "Ciel clair";
            }
            else if (cloudiness <= 20)
            {
                return "Quelques nuages";
            }
            else if (cloudiness <= 50)
            {
                return "Partiellement nuageux";
            }
            else if (cloudiness <= 80)
            {
                return "Nuageux";
            }
            else
            {
                return "Très nuageux";
            }
        }



            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Clouds
        {
            public int all { get; set; }
        }

        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public int sea_level { get; set; }
            public int grnd_level { get; set; }
        }

        public class Root
        {
            public Coord coord { get; set; }
            public List<Weather> weather { get; set; }
            public string @base { get; set; }
            public Main main { get; set; }
            public int visibility { get; set; }
            public Wind wind { get; set; }
            public Clouds clouds { get; set; }
            public int dt { get; set; }
            public Sys sys { get; set; }
            public int timezone { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
        }

        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public string country { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
            public int deg { get; set; }
        }

    }
}

 

