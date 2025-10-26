using ClassLibrary;
using System;

namespace ClassLibrary
{
    [Serializable]
    public class Cow : Animal
    {
        public Cow() { } 

        public Cow(string name, string country)
            : base(name, country)
        {
            WhatAnimal = "Cow";
            Classification = eClassificationAnimal.Herbivores;
        }

        public override eFavoriteFood GetFavouriteFood() => eFavoriteFood.Plants;

        public override void SayHello()
        {
            Console.WriteLine($"Moo! I'm {Name} from {Country}!");
        }
    }
}

