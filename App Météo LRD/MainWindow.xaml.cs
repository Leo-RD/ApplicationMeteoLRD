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
using Apiclasses;

namespace App_Météo_LRD
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadWeather();
        }

        public async Task<WeatherResponse> GetWeather() // Nouvelle méthode pour récupérer les données météo
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("https://www.prevision-meteo.ch/services/json/Annecy");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherResponse>(content);
            }
            else
            {
                return null;
            }
        }
        private async void LoadWeather() // Mise à jour pour utiliser la nouvelle API
        {
            Root weatherData = await GetWeather();
            if (weatherData != null)
            {
                TemperatureTextBlock.Text = $"Température: {weatherData.current_condition.tmp}°C";
                HourTextBlock.Text = $"Ressenti: {weatherData.current_condition.hour}°C";
                TempMinTextBlock.Text = $"Température Min: {weatherData.fcst_day_0.tmin}°C";
                TempMaxTextBlock.Text = $"Température Max: {weatherData.fcst_day_0.tmax}°C";
                HumidityTextBlock.Text = $"Humidité: {weatherData.current_condition.humidity}%";
                CloudTextBlock.Text = weatherData.current_condition.condition;
            }
            else
            {
                TemperatureTextBlock.Text = "Erreur de récupération des données";
            }
        }


        public class WeatherResponse 
        {
            public CurrentCondition CurrentCondition { get; set; }
            public FcstDay0 fcst_day_0 { get; set; }
        }
    }
}
