using System;


namespace ConsoleApp2;

public class Currency
{ 
    public decimal Value {get; set;  }

    public Currency(decimal value)
    {
        Value = value;
    }
}

public class CurrencyUSD : Currency
{
    public static decimal USDtoEUR { get; set; }
    public static decimal USDtoRUB { get; set; }

    public CurrencyUSD(decimal value) : base(value) { }

    public static implicit operator CurrencyEUR(CurrencyUSD usd)
    {
        return new CurrencyEUR(usd.Value * USDtoEUR);
    }

    public static implicit operator CurrencyRUB(CurrencyUSD usd)
    {
        return new CurrencyRUB(usd.Value * USDtoRUB);
    }
}

public class CurrencyEUR : Currency
{
    public static decimal EURtoUSD { get; set; }
    public static decimal EURtoRUB { get; set; }

    public CurrencyEUR(decimal value) : base(value) { }

    public static implicit operator CurrencyUSD(CurrencyEUR eur)
    {
        return new CurrencyUSD(eur.Value * EURtoUSD);
    }

    public static implicit operator CurrencyRUB(CurrencyEUR eur)
    {
        return new CurrencyRUB(eur.Value * EURtoRUB);
    }
}

public class CurrencyRUB : Currency
{
    public static decimal RUBtoUSD { get; set; }
    public static decimal RUBtoEUR { get; set; }

    public CurrencyRUB(decimal value) : base(value) { }

    public static implicit operator CurrencyUSD(CurrencyRUB rub)
    {
        return new CurrencyUSD(rub.Value * RUBtoUSD);
    }

    public static implicit operator CurrencyEUR(CurrencyRUB rub)
    {
        return new CurrencyEUR(rub.Value * RUBtoEUR);
    }
}

class Program
{
    static void Main()
    {
        CurrencyUSD.USDtoEUR = 0.93m;
        CurrencyUSD.USDtoRUB = 82.5m;

        CurrencyEUR.EURtoUSD = 1.08m;
        CurrencyEUR.EURtoRUB = 88.0m;

        CurrencyRUB.RUBtoUSD = 0.012m;
        CurrencyRUB.RUBtoEUR = 0.011m;


        CurrencyUSD usd = new CurrencyUSD(100);


        CurrencyEUR eur = usd;
        CurrencyRUB rub = usd;

        Console.WriteLine($"USD: {usd.Value}");
        Console.WriteLine($"EUR: {eur.Value}");
        Console.WriteLine($"RUB: {rub.Value}");


        CurrencyUSD usd2 = eur;
        Console.WriteLine($"EUR -> USD: {usd2.Value}");
    }
}
