using System;

namespace ConsoleApp2;

public class MyMatrix
{
    private int[,] data;
    private int rows;
    private int cols;
    private static Random rand = new Random();

    public MyMatrix(int rows, int cols, int minValue, int maxValue)
    {
        this.rows = rows;
        this.cols = cols;
        data = new int[rows, cols];

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                data[i, j] = rand.Next(minValue, maxValue + 1);
    }

    public int this[int i, int j]
    {
        get => data[i, j];
        set => data[i, j] = value;
    }

    public static MyMatrix operator +(MyMatrix a, MyMatrix b)
    {
        if (a.rows != b.rows || a.cols != b.cols)
            throw new ArgumentException("Матрицы должны быть одинакового размера");

        MyMatrix result = new MyMatrix(a.rows, a.cols, 0, 0);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < a.cols; j++)
                result[i, j] = a[i, j] + b[i, j];

        return result;
    }

    public static MyMatrix operator -(MyMatrix a, MyMatrix b)
    {
        if (a.rows != b.rows || a.cols != b.cols)
            throw new ArgumentException("Матрицы должны быть одинакового размера");

        MyMatrix result = new MyMatrix(a.rows, a.cols, 0, 0);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < a.cols; j++)
                result[i, j] = a[i, j] - b[i, j];

        return result;
    }

    public static MyMatrix operator *(MyMatrix a, MyMatrix b)
    {
        if (a.cols != b.rows)
            throw new ArgumentException("Число столбцов первой матрицы должно совпадать с числом строк второй");

        MyMatrix result = new MyMatrix(a.rows, b.cols, 0, 0);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < b.cols; j++)
            {
                int sum = 0;
                for (int k = 0; k < a.cols; k++)
                    sum += a[i, k] * b[k, j];
                result[i, j] = sum;
            }

        return result;
    }

    public static MyMatrix operator *(MyMatrix a, int number)
    {
        MyMatrix result = new MyMatrix(a.rows, a.cols, 0, 0);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < a.cols; j++)
                result[i, j] = a[i, j] * number;

        return result;
    }

    public static MyMatrix operator /(MyMatrix a, int number)
    {
        if (number == 0) throw new DivideByZeroException();

        MyMatrix result = new MyMatrix(a.rows, a.cols, 0, 0);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < a.cols; j++)
                result[i, j] = a[i, j] / number;

        return result;
    }

    public void Print()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                Console.Write($"{data[i, j],5}");
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите число строк: ");
        int rows = int.Parse(Console.ReadLine());

        Console.Write("Введите число столбцов: ");
        int cols = int.Parse(Console.ReadLine());

        Console.Write("Введите минимальное значение: ");
        int min = int.Parse(Console.ReadLine());

        Console.Write("Введите максимальное значение: ");
        int max = int.Parse(Console.ReadLine());

        MyMatrix m1 = new MyMatrix(rows, cols, min, max);
        MyMatrix m2 = new MyMatrix(rows, cols, min, max);

        Console.WriteLine("Матрица 1:");
        m1.Print();

        Console.WriteLine("Матрица 2:");
        m2.Print();

        Console.WriteLine("Сумма:");
        (m1 + m2).Print();

        Console.WriteLine("Разность:");
        (m1 - m2).Print();

        Console.WriteLine("Умножение на число (m1 * 2):");
        (m1 * 2).Print();
    }
}
