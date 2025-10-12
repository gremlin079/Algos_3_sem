using System;
using System.Collections;
using System.Collections.Generic;

class MyList<T> : IEnumerable<T>
{
    private T[] items;
    private int count;

    public int Count => count;

    public MyList()
    {
        items = new T[4];
        count = 0;
    }

    public void Add(T item)
    {
        if (count == items.Length)
            Resize(items.Length * 2);

        items[count] = item;
        count++;
    }

    private void Resize(int newSize)
    {
        T[] newArray = new T[newSize];
        for (int i = 0; i < count; i++)
            newArray[i] = items[i];
        items = newArray;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException("Индекс вне диапазона!");
            return items[index];
        }
        set
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException("Индекс вне диапазона!");
            items[index] = value;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
            yield return items[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

class Program
{
    static void Main()
    {
        var numbers = new MyList<int> { 10, 20, 30, 40 };

        Console.WriteLine("Элементы списка:");
        for (int i = 0; i < numbers.Count; i++)
        {
            Console.WriteLine($"numbers[{i}] = {numbers[i]}");
        }

        Console.WriteLine($"\nОбщее количество элементов: {numbers.Count}");

        numbers.Add(50);
        Console.WriteLine($"После добавления: Count = {numbers.Count}, последний элемент = {numbers[4]}");

        numbers[0] = 999;
        Console.WriteLine($"\nИзменённый первый элемент: {numbers[0]}");
    }
}
