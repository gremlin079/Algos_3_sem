using System;
using System.Diagnostics;
using System.Numerics;

namespace ConsoleApp2;

public class Vehicle
{
    private string coordinats;
    private int speed;
    private double count;
    private int data;

    public Vehicle(string coordinats, int speed, double count, int data)
    {
        this.coordinats = coordinats;
        this.speed = speed;
        this.count = count;
        this.data = data;
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine($"Координаты: {coordinats}");
        Console.WriteLine($"Цена: {count} USD");
        Console.WriteLine($"Скорость: {speed} км/ч");
        Console.WriteLine($"Год выпуска: {data}");
        Console.WriteLine(" ");
    }
}

public class Plane : Vehicle
{
    private int visota;
    private int pasad;

    public Plane(string coordinats, int speed, double count, int data, int visota, int pasad)
        :base(coordinats, speed, count, data)
    {
        this.visota = visota;
        this.pasad = pasad;
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Самолет:");
        base.ShowInfo();
        Console.WriteLine($"Высота: {visota}");
        Console.WriteLine($"Количество пасажиров: {pasad}");
        Console.WriteLine(" ");
    }
}

public class Car : Vehicle
{
    public Car(string coordinats, int speed, double count, int data)
        : base(coordinats, speed, count, data)
    { 
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Машина");
        base.ShowInfo();
        Console.WriteLine(" ");
    }
}


public class Ship : Vehicle
{
    private int pas;
    private string port;

    public Ship(string coordinats, int speed, double count, int data, int pas, string port)
        : base(coordinats, speed, count, data)
    {
        this.pas = pas;
        this.port = port;
    }

    public override void ShowInfo()
    {
        Console.WriteLine("Лодка");
        base.ShowInfo();
        Console.WriteLine($"Количесвто пасажиров: {pas}");
        Console.WriteLine($"Порт: {port}");
        Console.WriteLine(" ");
    }
}

class Program
{
    static void Main()
    {
        Vehicle car = new Car("12414.12324", 180, 20000, 2020);
        Vehicle plane = new Plane("83414.213414", 560, 30000, 2021, 10000, 17);
        Vehicle ship = new Ship("94354.14141", 100, 25000, 2019, 10, "Мурманский");
        
        car.ShowInfo();
        plane.ShowInfo();
        ship.ShowInfo();
    }
}
