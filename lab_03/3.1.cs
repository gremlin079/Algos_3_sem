using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ConsoleApp2;

public struct Vector
{
    private int x;
    private int y;
    private int z;

    public Vector(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    private double Len()
    {
        return Math.Sqrt(x * x + y * y + z * z);
    }
    
    public static Vector operator +(Vector vec1, Vector vec2)
    {
        return new Vector(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
    }

    public static int operator *(Vector vec1, Vector vec2)
    {
        return (vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z);
    }

    public static Vector operator *(Vector vec1, int chislo)
    {
        return new Vector(vec1.x * chislo, vec1.y * chislo, vec1.z * chislo);
    }

    public static bool operator >(Vector vec1, Vector vec2)
    {
        return vec1.Len() > vec2.Len();
    }
    
    public static bool operator <(Vector vec1, Vector vec2)
    {
        return vec1.Len() < vec2.Len();
    }

    public void ShowVector()
    {
        Console.WriteLine($"X: {x}");
        Console.WriteLine($"Y: {y}");
        Console.WriteLine($"Z: {z}");
        Console.WriteLine("");
    }
}

class Program
{
    static void Main()
    {
        Vector vec1 = new Vector(1, 2, 3);
        Vector vec2 = new Vector(3, 4, 5);

        Vector vec3 = vec1 + vec2;
        vec3.ShowVector();

        Console.WriteLine(vec1 * vec2);
        Console.WriteLine("");

        Vector vec4 = vec1 * 10;
        vec4.ShowVector();

        Console.WriteLine(vec2 > vec1);
    }
}
