using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    [Comment("Represents a pig (omnivore).")]
    public class Pig : Animal
    {
        public Pig() { }

        public Pig(string name, string country) : base(name, country)
        {
            WhatAnimal = "Pig";
            Classification = eClassificationAnimal.Omnivores;
        }

        public override eFavoriteFood GetFavouriteFood() => eFavoriteFood.Everything;

        public override void SayHello()
        {
            Console.WriteLine("Oink! I am a pig!");
        }
    }
}
