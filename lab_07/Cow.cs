using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    [Comment("Represents a cow (herbivore).")]
    public class Cow : Animal
    {
        public Cow(string name, string country) : base(name, country)
        {
            WhatAnimal = "Cow";
            Classification = eClassificationAnimal.Herbivores;
        }

        public override eFavoriteFood GetFavouriteFood() => eFavoriteFood.Plants;

        public override void SayHello()
        {
            Console.WriteLine("Moo! I am a cow!");
        }
    }
}
