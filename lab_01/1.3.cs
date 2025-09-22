using System;

namespace ConsoleApp2;

public class Point
{
    private int x;
    private int y;

    public int X => x;
    public int Y => y;


    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}


public class Figure
{
    private Point[] points;
    public string Name { get; set;  }
    
    public Figure(Point a, Point b, Point c, string name)
    {
        Name = name;
        points = new Point[] { a, b, c };
    }

    public Figure(Point a, Point b, Point c, Point d, string name)
        : this(a, b, c, name)
    {
        Array.Resize(ref points, 4);
        points[3] = d;
    }

    public Figure(Point a, Point b, Point c, Point d, Point e, string name)
        : this(a, b, c, d, name)
    {
        Array.Resize(ref points, 5);
        points[4] = e;
    }

    public double LengthSide(Point a, Point b)
    {
        return Math.Pow(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2), 0.5);
    }


    public double PerimeterCalculator()
    {
        double result = 0;
        for (int i = 0; i < points.Length - 1; i++)
        {
            result += LengthSide(points[i], points[i + 1]);
        }

        result += LengthSide(points[points.Length - 1], points[0]);
        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Point a = new Point(0, 0);
        Point b = new Point(0, 3);
        Point c = new Point(4, 0);
        Figure triangle = new Figure(a, b, c, "Triangle");
        Console.WriteLine($"Имя: {triangle.Name}");
        Console.WriteLine($"Периметр: {triangle.PerimeterCalculator()}");

        Point d = new Point(4, 3);
        Figure rectangle = new Figure(a, b, c, d, "Rectangle");
        Console.WriteLine($"Имя: {rectangle.Name}");
        Console.WriteLine($"Периметр: {rectangle.PerimeterCalculator()}");

        Point e = new Point(2, 5);
        Figure pentagon = new Figure(a, b, c, d, e, "Pentagon");
        Console.WriteLine($"Имя: {pentagon.Name}");
        Console.WriteLine($"Периметр: {pentagon.PerimeterCalculator()}");

    }
}
