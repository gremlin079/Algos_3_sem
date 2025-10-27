using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

class Program
{
    const int MAX_CONCURRENT_REQUESTS = 2;
    const int MAX_RETRIES = 3;
    static readonly TimeSpan RETRY_DELAY = TimeSpan.FromSeconds(2);
    static readonly object fileLock = new object();

    static async Task Main(string[] args)
    {
        string token = "UHFneTNMTEdiZm9USWk4VUdkTVVHQWtxT0ozTndJNENSVHRlMjdzNklHMD0";
        string tickerFile = "ticker.txt";

        var tickers = File.ReadAllLines(tickerFile, System.Text.Encoding.UTF8);

        var tickerList = new ConcurrentQueue<string>();
        foreach (var t in tickers)
        {
            var s = t?.Trim();
            if (!string.IsNullOrEmpty(s))
                tickerList.Enqueue(s);
        }

        if (tickerList.IsEmpty)
        {
            Console.WriteLine("Список тикеров пуст.");
            return;
        }

        string outputFile = "result.txt";

        if (File.Exists(outputFile)) File.Delete(outputFile);

        using var http = new HttpClient();
        http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var semaphore = new SemaphoreSlim(MAX_CONCURRENT_REQUESTS);
        var tasks = new ConcurrentBag<Task>();

        while (tickerList.TryDequeue(out var ticker))
        {
            await semaphore.WaitAsync();

            var task = Task.Run(async () =>
            {
                try
                {
                    double? avg = await GetYearlyAverageAsync(http, ticker);
                    if (avg.HasValue)
                    {
                        string line = $"{ticker}:{avg.Value:F4}";
                        await SafeAppendLineAsync(outputFile, line);
                        Console.WriteLine(line);
                    }
                    else
                    {
                        string line = $"{ticker}:NO_DATA";
                        await SafeAppendLineAsync(outputFile, line);
                        Console.WriteLine(line);
                    }
                }
                catch (Exception ex)
                {
                    string errLine = $"{ticker}:ERROR ({ex.Message})";
                    await SafeAppendLineAsync(outputFile, errLine);
                    Console.WriteLine(errLine);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        Console.WriteLine("Готово. Результаты в " + Path.GetFullPath(outputFile));
    }

    static async Task SafeAppendLineAsync(string filePath, string line)
    {
        lock (fileLock)
        {
            File.AppendAllText(filePath, line + Environment.NewLine);
        }
        await Task.CompletedTask;
    }


    static async Task<double?> GetYearlyAverageAsync(HttpClient http, string ticker)
    {
        DateTime to = DateTime.UtcNow.Date;
        DateTime from = to.AddMonths(-3);

        string url = $"https://api.marketdata.app/v1/stocks/candles/D/{Uri.EscapeDataString(ticker)}/?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";

        for (int attempt = 1; attempt <= MAX_RETRIES; attempt++)
        {
            try
            {
                using var resp = await http.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    string body = await resp.Content.ReadAsStringAsync();
                    if (resp.StatusCode == System.Net.HttpStatusCode.NoContent ||
                        resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return null;

                    if ((int)resp.StatusCode == 429)
                    {
                        await Task.Delay(RETRY_DELAY * attempt);
                        continue;
                    }

                    Console.WriteLine($"[{ticker}] HTTP {(int)resp.StatusCode}. Тело ответа: {Shorten(body, 200)}");
                    return null;
                }

                var json = await resp.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("s", out var sProp))
                {
                    var sVal = sProp.GetString();
                    if (sVal == "no_data" || sVal == "no symbol")
                        return null;
                }

                if (root.TryGetProperty("h", out var highs) && root.TryGetProperty("l", out var lows)
                    && highs.ValueKind == JsonValueKind.Array && lows.ValueKind == JsonValueKind.Array)
                {
                    int nHigh = highs.GetArrayLength();
                    int nLow = lows.GetArrayLength();
                    int n = Math.Min(nHigh, nLow);
                    if (n == 0) return null;

                    double sum = 0;
                    int count = 0;
                    for (int i = 0; i < n; i++)
                    {
                        double h = highs[i].GetDouble();
                        double l = lows[i].GetDouble();
                        if (double.IsNaN(h) || double.IsNaN(l)) continue;
                        sum += (h + l) / 2.0;
                        count++;
                    }
                    if (count == 0) return null;
                    return sum / count;
                }

            }
            catch (JsonException jex)
            {
                Console.WriteLine($"[{ticker}] JSON parse error: {jex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{ticker}] Ошибка запроса: {ex.Message}. Попытка {attempt}/{MAX_RETRIES}");
                if (attempt < MAX_RETRIES) await Task.Delay(RETRY_DELAY * attempt);
            }
        }

        return null;
    }

    static string Shorten(string s, int max)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return s.Length <= max ? s : s.Substring(0, max) + "...";
    }
}
