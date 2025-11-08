using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

class Program
{
    const int PORT = 5000;
    static readonly string connectionString = "Data Source=stocks.db";

    static async Task Main()
    {
        var listener = new TcpListener(IPAddress.Loopback, PORT);
        listener.Start();
        Console.WriteLine($"Сервер запущен на 127.0.0.1:{PORT}");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = Task.Run(() => HandleClient(client));
        }
    }

    static async Task HandleClient(TcpClient client)
    {
        using var stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
        string ticker = Encoding.UTF8.GetString(buffer, 0, bytes).Trim().ToUpper();

        Console.WriteLine($"Получен тикер: {ticker}");

        string response = GetLatestPrice(ticker);
        if (response == null)
            response = "Нет данных по этому тикеру";

        byte[] data = Encoding.UTF8.GetBytes(response);
        await stream.WriteAsync(data, 0, data.Length);
        Console.WriteLine($"Ответ отправлен: {response}");
    }

    static string GetLatestPrice(string ticker)
    {
        try
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT p.price, p.date
                FROM Prices p
                JOIN Tickers t ON p.tickerId = t.id
                WHERE t.ticker = $ticker
                ORDER BY p.date DESC LIMIT 1";
            cmd.Parameters.AddWithValue("$ticker", ticker);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                double price = reader.GetDouble(0);
                string date = reader.GetString(1);
                return $"{ticker}: {price:F2} USD (на {date})";
            }
        }
        catch (Exception ex)
        {
            return $"Ошибка: {ex.Message}";
        }

        return null;
    }
}

