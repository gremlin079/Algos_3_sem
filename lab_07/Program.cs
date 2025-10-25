using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ClassLibrary;

namespace ReflectionApp
{
    class Program
    {
        static void Main()
        {
            var assembly = Assembly.Load("ClassLibrary");
            var types = assembly.GetTypes().Where(t => t.IsClass);

            var xml = new XElement("Library",
                from type in types
                select new XElement("Class",
                    new XAttribute("Name", type.Name),
                    new XAttribute("BaseType", type.BaseType?.Name ?? "None"),
                    new XAttribute("Namespace", type.Namespace),
                    new XAttribute("Comment",
                        type.GetCustomAttribute<CommentAttribute>()?.Comment ?? "No comment"),

                    new XElement("Properties",
                        from prop in type.GetProperties()
                        select new XElement("Property",
                            new XAttribute("Name", prop.Name),
                            new XAttribute("Type", prop.PropertyType.Name)
                        )
                    ),

                    new XElement("Methods",
                        from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        select new XElement("Method",
                            new XAttribute("Name", method.Name),
                            new XAttribute("ReturnType", method.ReturnType.Name)
                        )
                    )
                )
            );

            xml.Save("AnimalsStructure.xml");
            Console.WriteLine("XML file 'AnimalsStructure.xml' has been generated successfully!");
        }
    }
}