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
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string Bday { get; set; }
        public string Post { get; set; }
        public double Wage { get; set; }
        public double Bounty { get; set; }
        public double Earnings { get; set; }
        public double TotalPayment { get; set; }
        public abstract void Payment ();
        public override string ToString() //overrides ToString() method in order to output string that contains multiple vars
        {
            return ID + " " + Type + " " + Surname + " " + Name + " " + LastName + " " + Bday + " " + Post + " " + Wage + " " + Bounty + " " + Earnings;
        }
        public class Waged : OrganisationBC //derived from abstract class organisationbc
        {
            public override void Payment()
            {
                Earnings = Wage * 20.8 * 8 + Bounty;
            }
        }
        public class Salaried : OrganisationBC
        {
            public override void Payment()
            {
                Earnings = Wage + Bounty;
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
                string line = Console.ReadLine();
                string[] fields = line.Split(' ');
            wrongpayment:
                if (fields[1] == "Waged")
                {
                    Waged waged = new Waged { ID = int.Parse(fields[0]), Type = fields[1], Surname = fields[2], Name = fields[3], LastName = fields[4], Bday = fields[5], Post = fields[6], Wage = double.Parse(fields[7]), Bounty = double.Parse(fields[8])};
                    waged.Payment();
                    VoidMain.EmployeesList.Add(waged);
                }
                else if (fields[1] == "Salaried")
                {
                    Salaried salaried = new Salaried { ID = int.Parse(fields[0]), Type = fields[1], Surname = fields[2], Name = fields[3], LastName = fields[4], Bday = fields[5], Post = fields[6], Wage = double.Parse(fields[7]), Bounty = double.Parse(fields[8])};
                    salaried.Payment();
                    VoidMain.EmployeesList.Add(salaried);
                }
                else
                {
                    Console.WriteLine("Wrong payment type! Specify it again separately: ");
                    fields[1] = Console.ReadLine();
                    goto wrongpayment;
                }
            }
            private static void SortList()
            {
                
            }
            private static int Main(string[] args)
            { 
                while (true)
                {
                    SortList();
                    badinput:
                    var i = Console.ReadLine();
                    switch (i)
                    {
                        case "0": return (0);
                        case "1":
                            {
                                Console.WriteLine();
                                Console.WriteLine("Surname Name LastName Bday Post Wage Bounty Earnings");
                                foreach (OrganisationBC listline in EmployeesList)
                                {
                                    Console.WriteLine(listline);
                                }
                            }
                            break;
                        case "2":
                            Console.WriteLine();
                            break;
                        case "3":
                            Console.WriteLine();
                            break;
                        case "4":
                            AddToList();
                            break;
                        case "5":
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