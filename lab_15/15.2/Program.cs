using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public interface ILogRepository
{
    void Log(string message);
    void Log(object data);
}

public class TextFileLogRepository : ILogRepository
{
    private readonly string _filePath;

    public TextFileLogRepository(string filePath)
    {
        _filePath = filePath;
        var directory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }

    public void Log(string message)
    {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        File.AppendAllLines(_filePath, new[] { logEntry });
    }

    public void Log(object data)
    {
        Log(data?.ToString() ?? "NULL");
    }
}

public class JsonFileLogRepository : ILogRepository
{
    private readonly string _filePath;
    private readonly List<object> _logs;

    public JsonFileLogRepository(string filePath)
    {
        _filePath = filePath;
        _logs = new List<object>();

        var directory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        LoadExistingLogs();
    }

    private void LoadExistingLogs()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                var logs = JsonSerializer.Deserialize<List<object>>(json);
                if (logs != null)
                    _logs.AddRange(logs);
            }
            catch
            {
                _logs.Clear();
            }
        }
    }

    public void Log(string message)
    {
        var logEntry = new
        {
            Timestamp = DateTime.Now,
            Message = message,
            Type = "Text"
        };

        _logs.Add(logEntry);
        SaveLogs();
    }

    public void Log(object data)
    {
        var logEntry = new
        {
            Timestamp = DateTime.Now,
            Data = data,
            Type = data?.GetType().Name ?? "Unknown"
        };

        _logs.Add(logEntry);
        SaveLogs();
    }

    private void SaveLogs()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(_logs, options);
        File.WriteAllText(_filePath, json);
    }
}

public class MyLogger
{
    private readonly List<ILogRepository> _repositories;

    public MyLogger()
    {
        _repositories = new List<ILogRepository>();
    }

    public void AddRepository(ILogRepository repository)
    {
        _repositories.Add(repository);
    }

    public void Log(string message)
    {
        foreach (var repository in _repositories)
        {
            repository.Log(message);
        }
    }

    public void Log(object data)
    {
        foreach (var repository in _repositories)
        {
            repository.Log(data);
        }
    }

    public void LogInfo(string message)
    {
        Log($"[INFO] {message}");
    }

    public void LogError(string message)
    {
        Log($"[ERROR] {message}");
    }

    public void LogWarning(string message)
    {
        Log($"[WARNING] {message}");
    }
}

public class UserService
{
    private readonly MyLogger _logger;

    public UserService(MyLogger logger)
    {
        _logger = logger;
    }

    public void CreateUser(string username, string email)
    {
        _logger.LogInfo($"Создание пользователя: {username}");

        var userData = new { Username = username, Email = email, CreatedAt = DateTime.Now };
        _logger.Log(userData);

        if (username.Length < 3)
        {
            _logger.LogWarning($"Слишком короткое имя пользователя: {username}");
        }

        _logger.LogInfo($"Пользователь {username} успешно создан");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== MyLogger Demo ===");

        var logger = new MyLogger();
        logger.AddRepository(new TextFileLogRepository("logs/application.log"));
        logger.AddRepository(new JsonFileLogRepository("logs/data.json"));

        logger.LogInfo("Приложение запущено");
        logger.LogWarning("Это тестовое предупреждение");
        logger.LogError("Это тестовая ошибка");

        var user = new { Id = 1, Name = "John Doe", Age = 30 };
        logger.Log(user);

        var product = new { Id = 100, Name = "Laptop", Price = 999.99m };
        logger.Log(product);

        var userService = new UserService(logger);
        userService.CreateUser("alice", "alice@example.com");
        userService.CreateUser("bo", "bo@example.com");

        logger.LogInfo("Приложение завершает работу");

        Console.WriteLine("Логи записаны в папку 'logs'");
        Console.WriteLine("Проверьте файлы:");
        Console.WriteLine("- logs/application.log (текстовый формат)");
        Console.WriteLine("- logs/data.json (JSON формат)");
        Console.WriteLine("\nНажмите Enter для выхода...");
        Console.ReadLine();
    }
}