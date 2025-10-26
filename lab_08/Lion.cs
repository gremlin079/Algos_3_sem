using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    [Comment("Represents a lion (carnivore).")]
    public class Lion : Animal
    {
        public Lion() { }

        public Lion(string name, string country) : base(name, country)
        {
            WhatAnimal = "Lion";
            Classification = eClassificationAnimal.Carnivores;
        }

        public override eFavoriteFood GetFavouriteFood() => eFavoriteFood.Meat;

        public override void SayHello()
        {
            Console.WriteLine("Roar! I am a lion!");
        }
    }
}
