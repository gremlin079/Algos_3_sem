using System;

namespace ConsoleApp1;

public class Rectangle
{
    private double SideA, SideB;
    public double Area => CalculateArea();
    public double Perimeter => CalculatePerimeter();

    public Rectangle(double A, double B)
    {
        SideA = A;
        SideB = B;
    }

    private double CalculateArea()
    {
        return SideA * SideB;
    }
    private double CalculatePerimeter()
    {
        return 2 * (SideA + SideB);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите длинны сторон:");
        double A = double.Parse(Console.ReadLine());
        double B = double.Parse(Console.ReadLine());

        Rectangle Start = new Rectangle(A, B);

        Console.WriteLine($"Площадь прямоугольника:  {Start.Area}");
        Console.WriteLine($"Периметр прямоугольника:  {Start.Perimeter}");
    }
}
