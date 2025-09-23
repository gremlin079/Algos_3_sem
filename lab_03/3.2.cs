using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace ConsoleApp2;

public class Car : IEquatable<Car>
{
    public string Name { get; set; }
    public string Engine { get; set; }
    public int MaxSpeed { get; set; }

    public Car(string Name, string Engine, int MaxSpeed)
    {
        this.Name = Name;
        this.Engine = Engine;
        this.MaxSpeed = MaxSpeed;
    }


    public override string ToString()
    {
        return Name;
    }

    public bool Equals(Car other)
    {
        if (other == null)
        {
            return false;
        }
        return Name == other.Name && Engine == other.Engine && MaxSpeed == other.MaxSpeed;
    }

    public override bool Equals(object obj)
    {
        if (obj is Car car)
        {
            return Equals(car);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Engine, MaxSpeed);
    }

}

public class CarsCatalog
{
    private List<Car> cars = new List<Car>();

    public void AddCar(Car car)
    {
        cars.Add(car);
    }

    public string this[int index]
    {
        get 
        {
            if (index < 0 || index > cars.Count)
            {
                throw new IndexOutOfRangeException("Неправильный индекс");
            }
            return $"{cars[index].Name}, двигатель: {cars[index].Engine}";
        }
    }
}

class Program
{
    static void Main()
    {
        Car car1 = new Car("BMW M3", "Бензиновый", 280);
        Car car2 = new Car("Tesla Model S", "Электрический", 250);
        Car car3 = new Car("BMW M3", "Бензиновый", 280);

        Console.WriteLine(car1);
        Console.WriteLine("");

        Console.WriteLine(car1.Equals(car2));
        Console.WriteLine(car1.Equals(car3));
        Console.WriteLine("");

        CarsCatalog catalog = new CarsCatalog();
        catalog.AddCar(car1);
        catalog.AddCar(car2);

        Console.WriteLine(catalog[0]);
        Console.WriteLine(catalog[1]);
        Console.WriteLine(catalog[2]);
    }
}
