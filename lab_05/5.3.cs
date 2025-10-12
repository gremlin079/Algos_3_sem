using System;
using System.Collections;
using System.Collections.Generic;

class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private TKey[] keys;
    private TValue[] values;
    private int count;

    public int Count => count;

    public MyDictionary()
    {
        keys = new TKey[4];
        values = new TValue[4];
        count = 0;
    }

    public void Add(TKey key, TValue value)
    {
        for (int i = 0; i < count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                throw new ArgumentException("Ключ уже существует в словаре!");
        }

        if (count == keys.Length)
            Resize(keys.Length * 2);

        keys[count] = key;
        values[count] = value;
        count++;
    }

    private void Resize(int newSize)
    {
        TKey[] newKeys = new TKey[newSize];
        TValue[] newValues = new TValue[newSize];

        for (int i = 0; i < count; i++)
        {
            newKeys[i] = keys[i];
            newValues[i] = values[i];
        }

        keys = newKeys;
        values = newValues;
    }

    public TValue this[TKey key]
    {
        get
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                    return values[i];
            }
            throw new KeyNotFoundException("Ключ не найден!");
        }
        set
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(keys[i], key))
                {
                    values[i] = value;
                    return;
                }
            }
            throw new KeyNotFoundException("Ключ не найден!");
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
        {
            yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

class Program
{
    static void Main()
    {
        var dict = new MyDictionary<string, int>();

        dict.Add("One", 1);
        dict.Add("Two", 2);
        dict.Add("Three", 3);

        Console.WriteLine("Элементы словаря:");
        foreach (var item in dict)
        {
            Console.WriteLine($"{item.Key} = {item.Value}");
        }

        Console.WriteLine($"\nОбщее количество элементов: {dict.Count}");

        Console.WriteLine($"\nЗначение по ключу 'Two': {dict["Two"]}");

        dict["Two"] = 22;
        Console.WriteLine($"Изменённое значение по ключу 'Two': {dict["Two"]}");
    }
}
