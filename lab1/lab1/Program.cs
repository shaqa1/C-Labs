using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.Linq;

namespace lab1
{
    [XmlInclude(typeof(Waged))]
    [XmlInclude(typeof(Salaried))]
    public abstract class OrganisationBC //abstract means that it's just a base class, it can contain abstract methods
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string Bday { get; set; }
        public string Post { get; set; }
        public double Wage { get; set; }
        public double Bounty { get; set; }
        public static double TotalPayment { get; set; }
        public abstract void Payment (int id);
        public override string ToString() //overrides ToString() method in order to output string that contains multiple vars
        {
            return ID + " " + Surname + " " + Name + " " + LastName + " " + Bday + " " + Post + " " + Wage + " " + Bounty;
        }
        /*
         if (type == false)
         {
             summary=20.8*8*stake + Bounty;
         }
         else
         {
            OrganisationBC.TotalPayment = 1;
             summary=stake;
         }
         */
        public class Waged : OrganisationBC //derived from abstract class organisationbc
        {
            public override void Payment(int id)
            {
                TotalPayment = 1;
                //int idx = VoidMain.EmployeesList.IndexOf("Nipun Tomar");
                throw new NotImplementedException();
            }
        }
        public class Salaried : OrganisationBC
        {
            public override void Payment(int id)
            {
                throw new NotImplementedException();
            }
        }
        internal class VoidMain
        {
            public static List<OrganisationBC> EmployeesList { get; } = new List<OrganisationBC>();
            private static void SerializeObject()
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(List<OrganisationBC>)); //create an xmlserializer to serialize string int xml text
                Stream filestream = new FileStream(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "hehe.xml"), FileMode.Create); //open a filestream
                XmlWriter Writer = new XmlTextWriter(filestream, Encoding.Unicode); //serialize using the XmlTextWriter
                Serializer.Serialize(Writer, VoidMain.EmployeesList);
                Writer.Close();
                Console.WriteLine("\nFile is accessible via path: \"" + Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/hehe.xml\""));
            }
            private static void AddToList()
            {

            }
            private static void SortList()
            {

            }
            private static int Main(string[] args)
            {
                VoidMain.EmployeesList.Add(new Salaried { ID = 1, Surname = "Ryabuhin", Name = "Kesha", LastName = "Andreevich", Bday = "09.04.2001", Post = "Janitor", Wage = 2281337.1488, Bounty = 1337.228 });
                VoidMain.EmployeesList.Add(new Waged { ID = 2, Surname = "Ilyas", Name = "Samsonovich", LastName = "Kopirkin", Bday = "11.01.2009", Post = "SEO NA'VI", Wage = 435634654, Bounty = 1324 });
                VoidMain.EmployeesList.Add(new Salaried { ID = 3, Surname = "Sergeevich", Name = "Artem", LastName = "Sergeevich", Bday = "12.12.2012", Post = "Bomzh", Wage = 767578488, Bounty = 133453 });
                while (true)
                {
                    SortList();
                    badinput:
                    var i = Console.Read();
                    switch (i)
                    {
                        case 0: return (0);
                        case 1:
                            {
                                Console.WriteLine();
                                Console.WriteLine("Surname Name LastName Bday Post Wage Bounty");
                                foreach (OrganisationBC listline in EmployeesList)
                                {
                                    Console.WriteLine(listline);
                                }
                            }
                            break;
                        case 2:
                            Console.WriteLine();
                            break;
                        case 3:
                            Console.WriteLine();
                            break;
                        case 4:
                            AddToList();
                            break;
                        case 5:
                            SerializeObject();
                            break;
                        default:
                            {
                                Console.WriteLine("Wrong choice! Try again:");
                                goto badinput;
                            }
                    }
                }
            }
        }
    }
}
