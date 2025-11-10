using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

class Program
{
    static readonly string connectionString = "Data Source=stocks.db";
    static readonly HttpClient http = new HttpClient();
    const string token = "UHFneTNMTEdiZm9USWk4VUdkTVVHQWtxT0ozTndJNENSVHRlMjdzNklHMD0";

    static async Task Main()
    {
        Console.WriteLine("=== Stock Analyzer with Database (Normalized) ===\n");

        InitDatabase();

        string[] tickers = File.ReadAllLines("ticker.txt");
        Console.WriteLine($"Загружено {tickers.Length} тикеров\n");

        foreach (var t in tickers[..Math.Min(3, tickers.Length)])
            AddTicker(t.Trim());

        foreach (var t in tickers[..Math.Min(3, tickers.Length)])
            await LoadPricesForTicker(t.Trim());

        AnalyzeConditions();

        Console.Write("\nВведите тикер для проверки: ");
        string userTicker = Console.ReadLine()?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(userTicker))
            ShowCondition(userTicker);
    }

    static void InitDatabase()
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS Tickers (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            ticker TEXT NOT NULL UNIQUE
        );

        CREATE TABLE IF NOT EXISTS Prices (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            tickerId INTEGER NOT NULL,
            price REAL NOT NULL,
            date TEXT NOT NULL,
            FOREIGN KEY (tickerId) REFERENCES Tickers(id)
        );

        CREATE TABLE IF NOT EXISTS TodaysCondition (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            tickerId INTEGER NOT NULL,
            state TEXT NOT NULL,
            FOREIGN KEY (tickerId) REFERENCES Tickers(id)
        );";
        cmd.ExecuteNonQuery();

        Console.WriteLine("База данных готова.\n");
    }

    static void AddTicker(string ticker)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT OR IGNORE INTO Tickers (ticker) VALUES ($t)";
        cmd.Parameters.AddWithValue("$t", ticker);
        cmd.ExecuteNonQuery();
    }

    static async Task LoadPricesForTicker(string ticker)
    {
        int tickerId = GetTickerId(ticker);
        if (tickerId == -1) return;

        DateTime to = DateTime.UtcNow.Date;
        DateTime from = to.AddDays(-5);

        string url = $"https://api.marketdata.app/v1/stocks/candles/D/{ticker}/?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";
        http.DefaultRequestHeaders.Clear();
        http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        try
        {
            string json = await http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("c", out var closes) || !root.TryGetProperty("t", out var times))
            {
                Console.WriteLine($"[{ticker}] Нет данных.");
                return;
            }

            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            for (int i = 0; i < closes.GetArrayLength(); i++)
            {
                double price = closes[i].GetDouble();
                long unix = times[i].GetInt64();
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(unix).DateTime.Date;

                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Prices (tickerId, price, date) VALUES ($id, $p, $d)";
                cmd.Parameters.AddWithValue("$id", tickerId);
                cmd.Parameters.AddWithValue("$p", price);
                cmd.Parameters.AddWithValue("$d", date.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine($"[{ticker}] Данные загружены.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{ticker}] Ошибка: {ex.Message}");
        }
    }

    static int GetTickerId(string ticker)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id FROM Tickers WHERE ticker = $t";
        cmd.Parameters.AddWithValue("$t", ticker);

        var result = cmd.ExecuteScalar();
        return result == null ? -1 : Convert.ToInt32(result);
    }

    static void AnalyzeConditions()
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var clear = conn.CreateCommand();
        clear.CommandText = "DELETE FROM TodaysCondition";
        clear.ExecuteNonQuery();

        var tickers = conn.CreateCommand();
        tickers.CommandText = "SELECT id, ticker FROM Tickers";
        using var reader = tickers.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string ticker = reader.GetString(1);

            var prices = conn.CreateCommand();
            prices.CommandText = @"SELECT price FROM Prices 
                                   WHERE tickerId = $id 
                                   ORDER BY date DESC LIMIT 2";
            prices.Parameters.AddWithValue("$id", id);

            using var r2 = prices.ExecuteReader();
            double? today = null, yesterday = null;
            if (r2.Read()) today = r2.GetDouble(0);
            if (r2.Read()) yesterday = r2.GetDouble(0);

            if (today.HasValue && yesterday.HasValue)
            {
                string state = today > yesterday ? "выросла" :
                               today < yesterday ? "упала" : "не изменилась";

                var ins = conn.CreateCommand();
                ins.CommandText = "INSERT INTO TodaysCondition (tickerId, state) VALUES ($id, $s)";
                ins.Parameters.AddWithValue("$id", id);
                ins.Parameters.AddWithValue("$s", state);
                ins.ExecuteNonQuery();

                Console.WriteLine($"[{ticker}] Цена {state}");
            }
        }
    }

    static void ShowCondition(string ticker)
    {
        int id = GetTickerId(ticker);
        if (id == -1)
        {
            Console.WriteLine("Такого тикера нет.");
            return;
        }

        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT state FROM TodaysCondition WHERE tickerId = $id";
        cmd.Parameters.AddWithValue("$id", id);

        var res = cmd.ExecuteScalar()?.ToString();
        if (res == null)
            Console.WriteLine("Нет данных по этому тикеру.");
        else
            Console.WriteLine($"Акция {ticker} сегодня: {res}");
    }
}


