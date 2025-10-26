using System;
using System.IO;
using System.IO.Compression;

namespace FileSearchAndCompress
{
    class Program
    {
        static void Main()
        {
            Console.Write("Введите путь для поиска: ");
            string path = Console.ReadLine();

            Console.Write("Введите имя файла для поиска (например, cow.xml): ");
            string fileName = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Указанный путь не существует!");
                return;
            }

            string[] files = Directory.GetFiles(path, fileName, SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            string foundFile = files[0];
            Console.WriteLine($"✅ Найден файл: {foundFile}");

            Console.WriteLine("\n=== Содержимое файла ===");
            using (FileStream fs = new FileStream(foundFile, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fs))
            {
                Console.WriteLine(reader.ReadToEnd());
            }

            string compressedFile = foundFile + ".gz";
            using (FileStream sourceStream = new FileStream(foundFile, FileMode.Open))
            using (FileStream targetStream = new FileStream(compressedFile, FileMode.Create))
            using (GZipStream gzip = new GZipStream(targetStream, CompressionMode.Compress))
            {
                sourceStream.CopyTo(gzip);
            }

            Console.WriteLine($"\n✅ Файл сжат и сохранён как: {compressedFile}");
        }
    }
}


