using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiKey = "e1118afa9659a940eab1db127a438361";

        public Form1()
        {
            InitializeComponent();
            LoadCities();
        }

        private void LoadCities()
        {
            string path = "city.txt";
            if (!File.Exists(path))
            {
                MessageBox.Show($"Файл {path} не найден.\nПоложи его в папку:\n{Directory.GetCurrentDirectory()}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                listCities.Items.Add(line.Trim());
            }
        }

        private async void btnGetWeather_Click(object sender, EventArgs e)
        {
            if (listCities.SelectedItem == null)
            {
                MessageBox.Show("Выберите город из списка.", "Информация");
                return;
            }

            string selected = listCities.SelectedItem.ToString();
            string[] parts = selected.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                MessageBox.Show("Некорректный формат строки в city.txt. Ожидается: 'Город\\tширота, долгота'");
                return;
            }

            string cityName = parts[0].Trim();
            string[] coords = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (coords.Length < 2)
            {
                MessageBox.Show($"Некорректные координаты у {cityName}");
                return;
            }

            if (!double.TryParse(coords[0].Trim(), System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double lat) ||
                !double.TryParse(coords[1].Trim(), System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double lon))
            {
                MessageBox.Show($"Ошибка чтения координат у {cityName}");
                return;
            }

            lblWeather.Text = "Загрузка...";
            try
            {
                var (temp, desc) = await GetWeatherAsync(lat, lon);
                lblWeather.Text = $"{cityName}\nТемпература: {temp:F1} °C\n{desc}";
            }
            catch (Exception ex)
            {
                lblWeather.Text = "Ошибка: " + ex.Message;
            }
        }

        private async Task<(double temp, string desc)> GetWeatherAsync(double lat, double lon)
        {
            string url =
                $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric&lang=ru";

            var json = await httpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            double temp = root.GetProperty("main").GetProperty("temp").GetDouble();
            string desc = root.GetProperty("weather")[0].GetProperty("description").GetString();
            return (temp, desc);
        }
    }
}

