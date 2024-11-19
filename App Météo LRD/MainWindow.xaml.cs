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
            LoadWeather("Annecy"); // Charge la météo par défaut pour Annecy
        }

        public async Task<Root> GetWeather(string city) // Accepte le nom de la ville
        {
            HttpClient client = new HttpClient();
            string url = $"https://www.prevision-meteo.ch/services/json/{city}"; // Modifie l'URL selon la ville
            HttpResponseMessage response = await client.GetAsync(url);
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

        private async void LoadWeather(string city) // Accepte le nom de la ville
        {
            Root root = await GetWeather(city);
            if (root != null && root.current_condition != null)
            {
                // Met à jour le texte du TextBlock avec le nom de la ville
                CityTextBlock.Text = $"AUJOURD'HUI - {root.city_info.name}"; 

                LundiTextBlock.Text = $"Aujourd'hui: {root.fcst_day_0.tmax}°C";
                MardiTextBlock.Text = $"Demain: {root.fcst_day_1.tmax}°C";
                MercrediTextBlock.Text = $"Après-demain: {root.fcst_day_2.tmax}°C";
                JeudiTextBlock.Text = $"Trois jours: {root.fcst_day_3.tmax}°C";
                HumidityTextBlock.Text = $"Humidité: {root.current_condition.humidity}%";
                CloudTextBlock.Text = root.current_condition.condition;
                TemperatureTextBlock.Text = $"{root.current_condition.tmp}°C";
            }
            else
            {
                TemperatureTextBlock.Text = "Erreur de récupération des données";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Récupère le nom de la ville depuis la TextBox
            string city = Recherche.Text.Trim(); // Enlève les espaces superflus
            if (!string.IsNullOrEmpty(city))
            {
                LoadWeather(city); // Charge la météo pour la ville saisie
            }
            else
            {
                MessageBox.Show("Veuillez entrer un nom de ville valide.");
            }
        }
    }
}
