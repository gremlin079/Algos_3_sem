using System;
using System.IO;
using System.Xml.Serialization;
using ClassLibrary;

namespace SerializationApp
{
    class Program
    {
        static void Main()
        {
            Cow cow = new Cow("Bessie", "Canada")
            {
                HideFromOtherAnimals = false
            };

            string filePath = "cow.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(Animal));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, cow);
            }

            Console.WriteLine("✅ Object serialized to cow.xml");

            Animal? deserializedCow;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                deserializedCow = (Animal?)serializer.Deserialize(fs);
            }

            Console.WriteLine("✅ Object deserialized from XML:");
            Console.WriteLine($"Name: {deserializedCow.Name}");
            Console.WriteLine($"Country: {deserializedCow.Country}");
            Console.WriteLine($"Classification: {deserializedCow.Classification}");
            deserializedCow.SayHello();
        }
    }
}
