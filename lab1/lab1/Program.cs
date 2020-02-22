using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace lab1
{
    [XmlInclude(typeof(Waged))]
    [XmlInclude(typeof(Salaried))]
    public abstract class OrganisationBC //abstract means that it's just a base class, it can contain abstract methods
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Post { get; set; }
        public double Wage { get; set; }
        /* public static double Payment (bool type, double stake, out double summary, out refrtotal)
         {
             if (type == false)
             {
                 summary=20.8*8*stake;
                 //rfrtotal=
             }
             else
             {
                 summary=stake;
             }
         }*/

        public class Waged : OrganisationBC //derived from abstract class organisationbc
        {

        }
        public class Salaried : OrganisationBC
        {

        }
        public class EmployeesList
        {
            public List<OrganisationBC> Employees { get; } = new List<OrganisationBC>();
        }
        internal class VoidMain
        {
            private static EmployeesList List = new EmployeesList();
            private static void SerializeObject()
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(List<OrganisationBC>)); //create an xmlserializer to serialize string int xml text
                Stream filestream = new FileStream(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "hehe.xml"), FileMode.Create); //open a filestream
                XmlWriter Writer = new XmlTextWriter(filestream, Encoding.Unicode); //serialize using the XmlTextWriter
                Serializer.Serialize(Writer, VoidMain.List.Employees);
                Writer.Close();
                Console.WriteLine("File is accessible via path: \"" + Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "hehe.xml\""));
            }
            private static void Main(string[] args)
            {
                VoidMain.List.Employees.Add(new Salaried { Name = "Kesha", Surname = "Ryabuhin", Post = "Janitor", Wage = 2281337.1488, });
                VoidMain.List.Employees.Add(new Waged { Name = "Artem", Surname = "Kalmyk", Post = "that guy from IT", Wage = 1337.7 });
                VoidMain.List.Employees.Add(new Waged { Name = "Keksik", Surname = "Ekler", Post = "SEO NA'VI", Wage = 228.8 });
                while (true)
                {
                    var i = Console.Read();
                    if (i == '1')
                    {
                        SerializeObject();
                    }
                    else if (i == '2')
                    {
                        Console.WriteLine("Pidor");
                    }

                }
            }
        }
    }
}

