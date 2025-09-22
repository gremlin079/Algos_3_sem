using System;

namespace ConsoleApp2;

public class DocumentWorker
{
    public virtual void OpenDocument()
    {
        Console.WriteLine("Документ открыт");
        Console.WriteLine(" ");
    }

    public virtual void EditDocument()
    {
        Console.WriteLine("Редактирование документа доступно в версии PRO");
        Console.WriteLine(" ");
    }

    public virtual void SaveDocument()
    {
        Console.WriteLine("Сохранение документа доступно в версии PRO");
        Console.WriteLine(" ");
    }
}

public class ProDocumentWorker:DocumentWorker
{
    public override void EditDocument()
    {
        Console.WriteLine("Документ отредактирован");
        Console.WriteLine(" ");
    }

    public override void SaveDocument()
    {
        Console.WriteLine("Документ сохранен в старом формате, сохранение в остальных форматах доступно в версии Expert");
        Console.WriteLine(" ");
    }
}

public class ExpertDocumentWorker : ProDocumentWorker
{
    public override void SaveDocument()
    {
        Console.WriteLine("Документ сохранен в новом формате");
        Console.WriteLine(" ");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите ключ доступа");
        string key = Console.ReadLine();

        if (key == "")
        {
            DocumentWorker worker = new DocumentWorker();
            worker.OpenDocument();
            worker.EditDocument();
            worker.SaveDocument();

        }

        else if (key == "pro")
        {
            DocumentWorker worker = new ProDocumentWorker();
            worker.OpenDocument();
            worker.EditDocument();
            worker.SaveDocument();

        }

        else if (key == "exp")
        {
            DocumentWorker worker = new ExpertDocumentWorker();
            worker.OpenDocument();
            worker.EditDocument();
            worker.SaveDocument();

        }

        else
        {
            Console.WriteLine("Неправильный ключ доступа");
        }
    }
}
