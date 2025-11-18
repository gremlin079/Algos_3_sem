using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

public interface IFileSystemObserver
{
    void OnFileCreated(string filePath);
    void OnFileDeleted(string filePath);
    void OnFileChanged(string filePath);
}

public interface IFileSystemWatcher
{
    void Attach(IFileSystemObserver observer);
    void Detach(IFileSystemObserver observer);
    void NotifyFileCreated(string filePath);
    void NotifyFileDeleted(string filePath);
    void NotifyFileChanged(string filePath);
}

public class SimpleFileSystemWatcher : IFileSystemWatcher, IDisposable
{
    private readonly List<IFileSystemObserver> _observers;
    private readonly System.Timers.Timer _timer;
    private readonly string _directoryPath;
    private Dictionary<string, DateTime> _fileLastWriteTimes;

    public SimpleFileSystemWatcher(string directoryPath, double interval = 1000)
    {
        _observers = new List<IFileSystemObserver>();
        _directoryPath = directoryPath;
        _fileLastWriteTimes = new Dictionary<string, DateTime>();

        _timer = new System.Timers.Timer(interval);
        _timer.Elapsed += CheckDirectory;

        ScanDirectory();
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Attach(IFileSystemObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Detach(IFileSystemObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyFileCreated(string filePath)
    {
        foreach (var observer in _observers)
        {
            observer.OnFileCreated(filePath);
        }
    }

    public void NotifyFileDeleted(string filePath)
    {
        foreach (var observer in _observers)
        {
            observer.OnFileDeleted(filePath);
        }
    }

    public void NotifyFileChanged(string filePath)
    {
        foreach (var observer in _observers)
        {
            observer.OnFileChanged(filePath);
        }
    }

    private void ScanDirectory()
    {
        if (!Directory.Exists(_directoryPath))
            return;

        var files = Directory.GetFiles(_directoryPath);
        foreach (var file in files)
        {
            var lastWriteTime = File.GetLastWriteTime(file);
            _fileLastWriteTimes[file] = lastWriteTime;
        }
    }

    private void CheckDirectory(object sender, ElapsedEventArgs e)
    {
        if (!Directory.Exists(_directoryPath))
            return;

        var currentFiles = new HashSet<string>(Directory.GetFiles(_directoryPath));
        var previousFiles = new HashSet<string>(_fileLastWriteTimes.Keys);

        foreach (var file in currentFiles)
        {
            if (!previousFiles.Contains(file))
            {
                _fileLastWriteTimes[file] = File.GetLastWriteTime(file);
                NotifyFileCreated(file);
            }
        }

        foreach (var file in previousFiles)
        {
            if (!currentFiles.Contains(file))
            {
                _fileLastWriteTimes.Remove(file);
                NotifyFileDeleted(file);
            }
        }

        foreach (var file in currentFiles)
        {
            var currentWriteTime = File.GetLastWriteTime(file);
            if (_fileLastWriteTimes.TryGetValue(file, out var previousWriteTime) &&
                currentWriteTime != previousWriteTime)
            {
                _fileLastWriteTimes[file] = currentWriteTime;
                NotifyFileChanged(file);
            }
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

public class FileLogger : IFileSystemObserver
{
    public void OnFileCreated(string filePath)
    {
        Console.WriteLine($"Файл создан: {filePath}");
    }

    public void OnFileDeleted(string filePath)
    {
        Console.WriteLine($"Файл удален: {filePath}");
    }

    public void OnFileChanged(string filePath)
    {
        Console.WriteLine($"Файл изменен: {filePath}");
    }
}

public class Program
{
    public static void Main()
    {
        using var watcher = new SimpleFileSystemWatcher(".");
        var logger = new FileLogger();

        watcher.Attach(logger);
        watcher.Start();

        Console.WriteLine("Наблюдение за директорией начато. Нажмите любую клавишу для остановки...");
        Console.ReadKey();

        watcher.Stop();
        Console.WriteLine("Наблюдение остановлено.");
    }
}