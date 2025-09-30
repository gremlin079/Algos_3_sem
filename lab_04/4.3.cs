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

public class CarCatalog
{
    private Car[] cars;

    public CarCatalog(Car[] cars)
    {
        this.cars = cars;
    }

    public IEnumerable<Car> Forward()
    {
        for (int i = 0; i < cars.Length; i++)
            yield return cars[i];
    }

    public IEnumerable<Car> Backward()
    {
        for (int i = cars.Length - 1; i >= 0; i--)
            yield return cars[i];
    }

    public IEnumerable<Car> FilterByYear(int year)
    {
        foreach (var car in cars)
            if (car.ProductionYear >= year)
                yield return car;
    }

    public IEnumerable<Car> FilterBySpeed(int speed)
    {
        foreach (var car in cars)
            if (car.MaxSpeed >= speed)
                yield return car;
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

        CarCatalog catalog = new CarCatalog(cars);

        Console.WriteLine("Прямой проход:");
        foreach (var car in catalog.Forward())
            Console.WriteLine(car);

        Console.WriteLine("\nОбратный проход:");
        foreach (var car in catalog.Backward())
            Console.WriteLine(car);

        Console.WriteLine("\nФильтр по году выпуска (>= 2015):");
        foreach (var car in catalog.FilterByYear(2015))
            Console.WriteLine(car);

        Console.WriteLine("\nФильтр по максимальной скорости (>= 230):");
        foreach (var car in catalog.FilterBySpeed(230))
            Console.WriteLine(car);
    }
}
