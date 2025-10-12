using System;

class MyMatrix
{
    private int[,] matrix;
    private int rows;
    private int cols;
    private int minValue;
    private int maxValue;
    private Random rand = new Random();

    public MyMatrix(int rows, int cols, int minValue, int maxValue)
    {
        this.rows = rows;
        this.cols = cols;
        this.minValue = minValue;
        this.maxValue = maxValue;
        matrix = new int[rows, cols];
        Fill();
    }

    public void Fill()
    {
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                matrix[i, j] = rand.Next(minValue, maxValue + 1);
    }

    public void ChangeSize(int newRows, int newCols)
    {
        int[,] newMatrix = new int[newRows, newCols];

        for (int i = 0; i < Math.Min(rows, newRows); i++)
            for (int j = 0; j < Math.Min(cols, newCols); j++)
                newMatrix[i, j] = matrix[i, j];

        for (int i = 0; i < newRows; i++)
            for (int j = 0; j < newCols; j++)
                if (i >= rows || j >= cols)
                    newMatrix[i, j] = rand.Next(minValue, maxValue + 1);

        matrix = newMatrix;
        rows = newRows;
        cols = newCols;
    }

    public void ShowPartialy(int startRow, int endRow, int startCol, int endCol)
    {
        Console.WriteLine($"Partial matrix ({startRow}-{endRow}, {startCol}-{endCol}):");
        for (int i = startRow; i <= endRow && i < rows; i++)
        {
            for (int j = startCol; j <= endCol && j < cols; j++)
                Console.Write($"{matrix[i, j],5}");
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public void Show()
    {
        Console.WriteLine("Full matrix:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                Console.Write($"{matrix[i, j],5}");
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public int this[int i, int j]
    {
        get
        {
            if (i < 0 || j < 0 || i >= rows || j >= cols)
                throw new IndexOutOfRangeException("Индекс вне диапазона матрицы!");
            return matrix[i, j];
        }
        set
        {
            if (i < 0 || j < 0 || i >= rows || j >= cols)
                throw new IndexOutOfRangeException("Индекс вне диапазона матрицы!");
            matrix[i, j] = value;
        }
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите количество строк: ");
        int rows = int.Parse(Console.ReadLine());
        Console.Write("Введите количество столбцов: ");
        int cols = int.Parse(Console.ReadLine());
        Console.Write("Введите минимальное значение: ");
        int minValue = int.Parse(Console.ReadLine());
        Console.Write("Введите максимальное значение: ");
        int maxValue = int.Parse(Console.ReadLine());

        MyMatrix m = new MyMatrix(rows, cols, minValue, maxValue);

        m.Show();

        Console.WriteLine("Частичный вывод (0–1 строки, 0–2 столбца):");
        m.ShowPartialy(0, 1, 0, 2);

        Console.WriteLine("Изменяем размер матрицы...");
        m.ChangeSize(rows + 1, cols + 2);
        m.Show();

        Console.WriteLine($"Элемент [0,0] = {m[0, 0]}");
        m[0, 0] = 999;
        Console.WriteLine($"Новый элемент [0,0] = {m[0, 0]}");
    }
}
