using System;
using System.Collections.Generic;

public class Car
{
    public string Name { get; set; }
    public int ProductionYear { get; set; }
    public int MaxSpeed { get; set; }

    public Car(string name, int year, int speed)
    {
        Name = name;
        ProductionYear = year;
        MaxSpeed = speed;
    }

    public override string ToString()
    {
        return $"{Name,-10} | {ProductionYear} | {MaxSpeed} км/ч";
    }
}

public class CarComparer : IComparer<Car>
{
    public enum CompareType
    {
        Name,
        ProductionYear,
        MaxSpeed
    }

    private CompareType compareType;

    public CarComparer(CompareType type)
    {
        compareType = type;
    }

    public int Compare(Car x, Car y)
    {
        if (x == null || y == null) return 0;

        return compareType switch
        {
            CompareType.Name => string.Compare(x.Name, y.Name, StringComparison.Ordinal),
            CompareType.ProductionYear => x.ProductionYear.CompareTo(y.ProductionYear),
            CompareType.MaxSpeed => x.MaxSpeed.CompareTo(y.MaxSpeed),
            _ => 0
        };
    }
}

class Program
{
    static void Main()
    {
        Car[] cars =
        {
            new Car("BMW", 2015, 250),
            new Car("Audi", 2018, 240),
            new Car("Lada", 2010, 160),
            new Car("Tesla", 2021, 300),
            new Car("Ford", 2012, 220)
        };

        Console.WriteLine("Исходный список:");
        PrintCars(cars);

        Array.Sort(cars, new CarComparer(CarComparer.CompareType.Name));
        Console.WriteLine("\nСортировка по названию:");
        PrintCars(cars);

        Array.Sort(cars, new CarComparer(CarComparer.CompareType.ProductionYear));
        Console.WriteLine("\nСортировка по году выпуска:");
        PrintCars(cars);

        Array.Sort(cars, new CarComparer(CarComparer.CompareType.MaxSpeed));
        Console.WriteLine("\nСортировка по максимальной скорости:");
        PrintCars(cars);
    }

    static void PrintCars(Car[] cars)
    {
        foreach (var car in cars)
            Console.WriteLine(car);
    }
}
