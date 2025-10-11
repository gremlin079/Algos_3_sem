using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;


public struct Weather
{
    public string Country { get; set; }
    public string Name { get; set; }
    public double Temp { get; set; }
    public string Description { get; set; }

    public override string ToString()
    {
        return $"{Country,-10} | {Name,-20} | {Temp,6:F2}°C | {Description}";
    }
}

class Program
{
    static async Task Main()
    {
        string apiKey = "e1118afa9659a940eab1db127a438361";
        int count = 50;
        var random = new Random();
        var httpClient = new HttpClient();
        var weatherList = new List<Weather>();

        Console.WriteLine("Получаем данные погоды...");

        while (weatherList.Count < count)
        {
            double lat = random.NextDouble() * 180 - 90;
            double lon = random.NextDouble() * 360 - 180; 

            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

            try
            {
                var response = await httpClient.GetStringAsync(url);
                using var doc = JsonDocument.Parse(response);

                var root = doc.RootElement;

                string country = root.GetProperty("sys").TryGetProperty("country", out var c) ? c.GetString() : null;
                string name = root.TryGetProperty("name", out var n) ? n.GetString() : null;

                if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(name))
                    continue;

                double temp = root.GetProperty("main").GetProperty("temp").GetDouble();
                string description = root.GetProperty("weather")[0].GetProperty("description").GetString();

                weatherList.Add(new Weather
                {
                    Country = country,
                    Name = name,
                    Temp = temp,
                    Description = description
                });

                Console.WriteLine($"{weatherList.Count,2}/50: {country,-2} | {name,-20} | {temp,6:F1}°C | {description}");
            }
            catch
            {
                continue;
            }

            await Task.Delay(200);
        }

        Console.WriteLine("\n=== Анализ данных ===\n");

        var maxTemp = weatherList.OrderByDescending(w => w.Temp).First();
        Console.WriteLine($"Максимальная температура: {maxTemp.Country} ({maxTemp.Name}) — {maxTemp.Temp:F1}°C");

        var minTemp = weatherList.OrderBy(w => w.Temp).First();
        Console.WriteLine($"Минимальная температура: {minTemp.Country} ({minTemp.Name}) — {minTemp.Temp:F1}°C");

        var avgTemp = weatherList.Average(w => w.Temp);
        Console.WriteLine($"Средняя температура в мире: {avgTemp:F2}°C");

        var countryCount = weatherList.Select(w => w.Country).Distinct().Count();
        Console.WriteLine($"Количество уникальных стран: {countryCount}");

        string[] targets = { "clear sky", "rain", "few clouds" };
        foreach (var d in targets)
        {
            var found = weatherList.FirstOrDefault(w => w.Description.Contains(d, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(found.Country))
                Console.WriteLine($"Первое вхождение '{d}': {found.Country} — {found.Name}");
        }
    }
}
