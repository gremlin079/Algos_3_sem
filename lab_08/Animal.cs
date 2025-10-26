using ClassLibrary;
using System;
using System.Xml.Serialization;

namespace ClassLibrary
{
    [Serializable]
    [XmlInclude(typeof(Cow))]
    [XmlInclude(typeof(Lion))]
    [XmlInclude(typeof(Pig))]
    public abstract class Animal
    {
        public string Country { get; set; }
        public bool HideFromOtherAnimals { get; set; }
        public string Name { get; set; }
        public string WhatAnimal { get; set; }
        public eClassificationAnimal Classification { get; set; }

        protected Animal() { }

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
    }
}

