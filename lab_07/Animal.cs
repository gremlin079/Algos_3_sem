using System;

namespace ClassLibrary
{
    [Comment("Abstract base class for all animals.")]
    public abstract class Animal
    {
        public string Country { get; set; }
        public bool HideFromOtherAnimals { get; set; }
        public string Name { get; set; }
        public string WhatAnimal { get; set; }
        public eClassificationAnimal Classification { get; set; }

        protected Animal(string name, string country)
        {
            Name = name;
            Country = country;
        }

        public abstract eFavoriteFood GetFavouriteFood();

        public virtual void SayHello()
        {
            Console.WriteLine($"Hello! I am {Name}, a {WhatAnimal} from {Country}.");
        }

        public eClassificationAnimal GetClassificationAnimal() => Classification;

        public void Deconstruct(out string name, out string country)
        {
            name = Name;
            country = Country;
        }
    }
}
