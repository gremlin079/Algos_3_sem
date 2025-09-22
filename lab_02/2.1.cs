using System;
using System.Numerics;

namespace ConsoleApp2;

public class ClasRoom
{
    private Pupil[] pupils = new Pupil[4];
    private int counter = 0;

    public ClasRoom(params Pupil[] args)
    {
        counter = args.Length;
        for (int i = 0; i < counter; i++)
        {
            pupils[i] = args[i];
        }
    }

    public void ShowParametr()
    {
        for (int i = 0; i < counter; i++)
        {
            Console.WriteLine($"Ученик номер {i + 1}:");
            pupils[i].Study();
            pupils[i].Read();
            pupils[i].Write();
            pupils[i].Relax();
            Console.WriteLine(" ");
        }
    }
}

public class Pupil
{
    public virtual void Study()
    {
        Console.WriteLine("Ученик учится.");
    }

    public virtual void Read()
    {
        Console.WriteLine("Ученик читает.");
    }

    public virtual void Write()
    {
        Console.WriteLine("Ученик пишет.");
    }

    public virtual void Relax()
    {
        Console.WriteLine("Ученик отдыхает.");
    }
}

public class ExcelentPupil : Pupil
{
    public override void Study()
    {
        Console.WriteLine("Ученик учится отлично.");
    }

    public override void Read()
    {
        Console.WriteLine("Ученик читает отлиично.");
    }

    public override void Write()
    {
        Console.WriteLine("Ученик пишет отлично.");
    }

    public override void Relax()
    {
        Console.WriteLine("Ученик отдыхает плохо.");
    }
}

public class GoodPupil : Pupil
{
    public override void Study()
    {
        Console.WriteLine("Ученик учится нормально.");
    }

    public override void Read()
    {
        Console.WriteLine("Ученик читает нормально.");
    }

    public override void Write()
    {
        Console.WriteLine("Ученик пишет нормально.");
    }

    public override void Relax()
    {
        Console.WriteLine("Ученик отдыхает неплохо.");
    }
}

public class BadPupil : Pupil
{
    public override void Study()
    {
        Console.WriteLine("Ученик учится плохо.");
    }

    public override void Read()
    {
        Console.WriteLine("Ученик читает плохо.");
    }

    public override void Write()
    {
        Console.WriteLine("Ученик пишет плохо.");
    }

    public override void Relax()
    {
        Console.WriteLine("Ученик отдыхает хорошо.");
    }
}

class Program
{
    static void Main()
    {
        ClasRoom room = new ClasRoom(new ExcelentPupil(), new GoodPupil(), new BadPupil());
        room.ShowParametr();
    }


}
