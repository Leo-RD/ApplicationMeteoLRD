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
using ApiClasses;

namespace App_Météo_LRD
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadWeather();
        }

        public async Task<Root> GetWeather() // Nouvelle méthode pour récupérer les données météo
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://www.prevision-meteo.ch/services/json/Annecy");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Root>(content);
            }
            else
            {
                return null;
            }
        }
        private async void LoadWeather() // Mise à jour pour utiliser la nouvelle API
        {
            Root root = await GetWeather();
            if (root != null && root.current_condition != null)
            {
                TemperatureTextBlock.Text = $"Température: {root.current_condition.tmp}°C";
                HourTextBlock.Text = $"Heure: {root.current_condition.hour}";
                TempMinTextBlock.Text = $"Température Min: {root.fcst_day_0.tmin}°C";
                TempMaxTextBlock.Text = $"Température Max: {root.fcst_day_0.tmax}°C";
                HumidityTextBlock.Text = $"Humidité: {root.current_condition.humidity}%";
                CloudTextBlock.Text = root.current_condition.condition;
            }
            else
            {
                TemperatureTextBlock.Text = "Erreur de récupération des données";
            }
            //WeatherResponse weatherData = await GetWeather();
            //if (weatherData != null && weatherData.currentCondition != null)
            //{
            //    TemperatureTextBlock.Text = $"Température: {weatherData.currentCondition.tmp}°C";
            //    HourTextBlock.Text = $"Ressenti: {weatherData.currentCondition.hour}°C";
            //    TempMinTextBlock.Text = $"Température Min: {weatherData.fcst_day_0.tmin}°C";
            //    TempMaxTextBlock.Text = $"Température Max: {weatherData.fcst_day_0.tmax}°C";
            //    HumidityTextBlock.Text = $"Humidité: {weatherData.currentCondition.humidity}%";
            //    CloudTextBlock.Text = weatherData.currentCondition.condition;
            //}
            //else
            //{
            //    TemperatureTextBlock.Text = "Erreur de récupération des données";
            //}
        }


      
    }
}
