using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class SingleRandomizer
{
    private static SingleRandomizer _instance;
    private static readonly object _lockObject = new object();
    private readonly Random _random;
    private readonly object _randomLock = new object();

    private SingleRandomizer()
    {
        _random = new Random();
    }

    public static SingleRandomizer Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new SingleRandomizer();
                    }
                }
            }
            return _instance;
        }
    }

    public int Next()
    {
        lock (_randomLock)
        {
            return _random.Next();
        }
    }

    public int Next(int maxValue)
    {
        lock (_randomLock)
        {
            return _random.Next(maxValue);
        }
    }

    public int Next(int minValue, int maxValue)
    {
        lock (_randomLock)
        {
            return _random.Next(minValue, maxValue);
        }
    }

    public double NextDouble()
    {
        lock (_randomLock)
        {
            return _random.NextDouble();
        }
    }

    public void NextBytes(byte[] buffer)
    {
        lock (_randomLock)
        {
            _random.NextBytes(buffer);
        }
    }
}

public class RandomizerTest
{
    public static void TestSingleThread()
    {
        Console.WriteLine("=== Тестирование в одном потоке ===");

        var randomizer = SingleRandomizer.Instance;

        Console.WriteLine("Случайные числа (1-100):");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"  {i + 1}. {randomizer.Next(1, 100)}");
        }

        Console.WriteLine("\nСлучайные дробные числа (0.0 - 1.0):");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"  {i + 1}. {randomizer.NextDouble():F4}");
        }

        Console.WriteLine("\nСлучайные байты:");
        byte[] bytes = new byte[5];
        randomizer.NextBytes(bytes);
        foreach (var b in bytes)
        {
            Console.Write($"{b} ");
        }
        Console.WriteLine();
    }

    public static void TestMultithreading()
    {
        Console.WriteLine("\n=== Тестирование в многопоточной среде ===");

        int taskCount = 5;
        var tasks = new Task[taskCount];

        Console.WriteLine($"Запуск {taskCount} параллельных задач...");

        for (int i = 0; i < tasks.Length; i++)
        {
            int taskId = i;
            tasks[i] = Task.Run(() =>
            {
                var randomizer = SingleRandomizer.Instance;
                Console.WriteLine($"Задача {taskId} началась");

                for (int j = 0; j < 3; j++)
                {
                    var number = randomizer.Next(1, 1000);
                    Console.WriteLine($"  Задача {taskId}, число {j + 1}: {number}");
                    Thread.Sleep(50);
                }

                Console.WriteLine($"Задача {taskId} завершена");
            });
        }

        Task.WaitAll(tasks);
        Console.WriteLine("Все задачи завершены");
    }

    public static void TestSingletonPattern()
    {
        Console.WriteLine("\n=== Проверка паттерна Одиночка ===");

        var instance1 = SingleRandomizer.Instance;
        var instance2 = SingleRandomizer.Instance;
        var instance3 = SingleRandomizer.Instance;

        Console.WriteLine($"instance1 == instance2: {instance1 == instance2}");
        Console.WriteLine($"instance2 == instance3: {instance2 == instance3}");
        Console.WriteLine($"instance1 == instance3: {instance1 == instance3}");

        Console.WriteLine($"Число из instance1: {instance1.Next(100)}");
        Console.WriteLine($"Число из instance2: {instance2.Next(100)}");
        Console.WriteLine($"Число из instance3: {instance3.Next(100)}");

        Console.WriteLine("Все экземпляры ссылаются на один и тот же объект!");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== SingleRandomizer Demo ===");

        RandomizerTest.TestSingleThread();

        RandomizerTest.TestSingletonPattern();

        RandomizerTest.TestMultithreading();

        Console.WriteLine("\nДемонстрация завершена. Нажмите Enter для выхода...");
        Console.ReadLine();
    }
}