using System;
using System.Collections.Generic;
//using System.Text;
using System.IO;
using System.Linq;
//using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace lab1
{
    [XmlInclude(typeof(Residential))]
    [XmlInclude(typeof(NonResidential))]
    public abstract class ManagementCompanyBC //abstract means that it's just a base class, it can contain abstract methods
    {
        public int Apartment { get; set; }
        public string Type { get; set; }
        public string Building { get; set; }
        public string Street { get; set; }
        public string LastName { get; set; }
        public string Bday { get; set; }
        public string Post { get; set; }
        public double Area { get; set; }
        public double Bounty { get; set; }
        public double ANoP { get; set; }
        //public double TotalPayment { get; set; }
        public abstract void AverageNumberOfPeople();
        public override string ToString() //overrides ToString() method in order to output string that contains multiple vars
        {
            string FormattedString = string.Format("{0,-16}{1,-22}{2,-11}{3,-12}{4,-11}{5,-6}", Type, Street, Building, Apartment, Area, ANoP);
            return FormattedString;
        }
        public class Residential : ManagementCompanyBC //derived from abstract class mancompbc
        {
            public override void AverageNumberOfPeople()
            {
                ANoP = Area * 20.8 * 8 + Bounty;
            }
        }
        public class NonResidential : ManagementCompanyBC
        {
            public override void AverageNumberOfPeople()
            {
                ANoP = Area * 0.2;
            }
        }
        internal class VoidMain
        {
            public static List<ManagementCompanyBC> PropertyList { get; } = new List<ManagementCompanyBC>();
            private static void SerializeObject() //file export
            {
                Console.Clear();
                Console.WriteLine("Enter file path and name followed by extension. Or you can leave file path blank and enter filename only, that will cause file to be created in OS default folder. Both JSON and XML extensions are supported:\n");
            wrongfileoutputextension:
                string filename = Console.ReadLine();
                if (!(filename.ToLower().IndexOf('\\') != -1))
                {
                    filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
                }
                if (filename.Substring(filename.Length - 4) == ".xml")
                {
                    using (StreamWriter filestream = File.CreateText(filename))
                    {
                        XmlSerializer XMLSerializer = new XmlSerializer(typeof(List<ManagementCompanyBC>));
                        XMLSerializer.Serialize(filestream, PropertyList);
                    }
                }
                else if (filename.Substring(filename.Length - 5) == ".json")
                { 
                    using (StreamWriter filestream = File.CreateText(filename))
                    {
                        JsonSerializer JSONSerializer = new JsonSerializer();
                        JSONSerializer.Serialize(filestream, PropertyList);
                    }
                } 
                else
                {
                    Console.WriteLine("Wrong file extension, please use .json and .xml only. Enter file name:\n");
                    goto wrongfileoutputextension;
                }
                Console.WriteLine("\nFile is accessible via path: \"" + filename + "\"\n\nPress any key to get back to main menu");
            }
            private static void DeSerializeObject() //file import
            {
                if (PropertyList.Any()) //check if list is not empty and promt to save current configuration
                {
                    Console.WriteLine("Current property list is not empty, do you want to save it? Type Y/N:");
                    if (Console.Read() == 'y' && Console.Read() == 'Y')
                    {
                        SerializeObject();
                        Console.WriteLine("Successfully saved.");
                    }

                }
                Console.WriteLine("Enter file path and name followed by extension. Or you can leave file path blank and enter filename only, path will be considered as OS default folder. Both JSON and XML extensions are supported:\n");
            wrongfileinputextension:
                string filename = Console.ReadLine();
                if (!(filename.ToLower().IndexOf('\\') != -1))
                {
                    filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
                }
                string UnparsedString = File.ReadAllText(filename);
                if (filename.Substring(filename.Length - 4) == ".xml")
                {
                    /*
                    using (StreamWriter filestream = File.CreateText(filename))
                    {
                        XmlSerializer XMLSerializer = new XmlSerializer(typeof(List<ManagementCompanyBC>));
                        XMLSerializer.Serialize(filestream, PropertyList);
                    }
                    */
                }
                else if (filename.Substring(filename.Length - 5) == ".json")
                {
                    
                   // VoidMain.PropertyList = JsonConvert.DeserializeObject<List<ManagementCompanyBC>>(UnparsedString);
                    /*
                    using (StreamWriter filestream = File.CreateText(filename))
                    {
                        JsonSerializer JSONSerializer = new JsonSerializer();
                        JSONSerializer.Serialize(filestream, PropertyList);
                    }
                    */
                }
                else
                {
                    Console.WriteLine("Wrong file extension, please use .json and .xml files only. Enter file name:\n");
                    goto wrongfileinputextension;
                }
                Console.WriteLine("\nFile is accessible via path: \"" + filename + "\"");
            }
            private static void AddToList()
            {
                string line = Console.ReadLine();
                string[] fields = line.Split(' ');
            wrongpayment:
                if (fields[0] == "Residential")
                {
                    Residential residential = new Residential { Type = fields[0], Street = fields[1], Building = fields[2], Apartment = int.Parse(fields[3]), Area = double.Parse(fields[4]) };
                    residential.AverageNumberOfPeople();
                    VoidMain.PropertyList.Add(residential);
                }
                else if (fields[0] == "Non-Residential")
                {
                    NonResidential nonresidential = new NonResidential { Type = fields[0], Street = fields[1], Building = fields[2], Apartment = int.Parse(fields[3]), Area = double.Parse(fields[4]) };
                    nonresidential.AverageNumberOfPeople();
                    VoidMain.PropertyList.Add(nonresidential);
                }
                else
                {
                    Console.WriteLine("Wrong property type! Specify it again: ");
                    fields[0] = Console.ReadLine();
                    goto wrongpayment;
                }
            }
            private static void SortList()
            {

            }
            private static void DrawMenu()
            {
                Console.Clear();
                Console.WriteLine("1. Show current list\n2. Show xxx files\n3. Show xxx files\n4. Add a new entry to property list\n5. Export current property list\n6. Import new property list from file\n0. Exit\n");
            }
            private static int Main(string[] args)
            {
                while (true)
                {
                    SortList();
                badinput:
                    DrawMenu();
                    var i = Console.ReadLine();
                    switch (i)
                    {
                        case "0": return (0);
                        case "1":
                            {
                                Console.Clear();
                                Console.WriteLine("Property type:  Street:               Building:  Apartment:  Area:      ANoP:  \n");
                                foreach (ManagementCompanyBC listline in PropertyList) //this will cause overrided ToString() method to be executed
                                {
                                    Console.WriteLine(listline);
                                }
                                Console.WriteLine("\nPress any key to get back to main menu");
                                Console.ReadKey();
                                DrawMenu();
                            }
                            break;
                        case "2":
                            { Console.WriteLine(); DrawMenu(); }
                            break;
                        case "3":
                            { Console.WriteLine(); DrawMenu(); }
                            break;
                        case "4":
                            { AddToList(); DrawMenu(); }
                            break;
                        case "5":
                            { SerializeObject(); Console.ReadKey(); DrawMenu(); }
                            break;
                        case "6":
                            { DeSerializeObject(); Console.ReadKey(); DrawMenu(); }
                            break;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("Wrong choice!\nPress any key to get back to main menu");
                                Console.ReadKey();
                                goto badinput;
                            }
                    }
                }
            }
        }
    }
}

/*
XmlSerializer XMLSerializer = new XmlSerializer(typeof(List<ManagementCompanyBC>)); //create an xmlserializer to serialize string int xml text
Stream filestream = new FileStream(path, FileMode.Create); //open a filestream
XmlWriter Writer = new XmlTextWriter(filestream, Encoding.Unicode); //serialize using the XmlTextWriter
XMLSerializer.Serialize(Writer, VoidMain.PropertyList);
Writer.Close();
         if (type == false)
         {
             summary=20.8*8*stake + Bounty;
         }
         else
         {
            OrganisationBC.TotalPayment = 1;
             summary=stake;
         }
                         //int idx = VoidMain.EmployeesList.IndexOf("Nipun Tomar");
                //throw new NotImplementedException();
                // foreach (OrganisationBC listline in VoidMain.EmployeesList)
                //{
                //TotalPayment += Earnings;
                //if (this.GetType().Name == "Waged") Console.WriteLine("WAGEDDDDDDDDDD");
                // if (this.GetType().Name == "Salaried") Console.WriteLine("SLRDDDDDDDDDDDDDDD");
                // Console.WriteLine(this.GetType().Name + "dddddddddddddddd");

                // }
         
 * //VoidMain.EmployeesList.Add(new Salaried { ID = 1, Type = "Waged", Surname = "Ryabuhin", Name = "Kesha", LastName = "Andreevich", Bday = "09.04.2001", Post = "Janitor", Wage = 2281337.1488, Bounty = 1337.228, Earnings = 111222 });
                //VoidMain.EmployeesList.Add(new Waged { ID = 2, Type = "Salaried", Surname = "Ilyas", Name = "Samsonovich", LastName = "Kopirkin", Bday = "11.01.2009", Post = "SEO NA'VI", Wage = 435634654, Bounty = 1324, Earnings = 111222 });
                VoidMain.EmployeesList.Add(new Salaried { ID = 3, Type = "Salaried", Surname = "Sergeevich", Name = "Artem", LastName = "Sergeevich", Bday = "12.12.2012", Post = "Bomzh", Wage = 767578488, Bounty = 133453, Earnings = 111222 });
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
     public static double Payment (bool type, double stake, out double summary, out refrtotal)
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
         }

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


 /* using System;
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
        public static double Payment (bool type, double stake, out double summary, out refrtotal)
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
        }
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
                Console.WriteLine("\nFile is accessible via path: \"" + Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/hehe.xml\""));
            }
            private static int Main(string[] args)
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
                        Console.WriteLine("\nPidor");
                    }
                    else if (i == '3')
                    {
                        Console.WriteLine("\nGnida");
                    }
                    else if (i == '9')
                    {
                        return (0);
                    }

                }
            }
}
    }
}



       //public abstract string Print(); 
        //public override string ToString();
    }
          //a non-abstract class derived from an abstract class must include actual implementations of all inherited abstract methods and accessors

        //public override string ToString()
        // {
        //      return $"{FirstName} {LastName}, {GroupName}";
        //  }
 *         //public float Rate { get; set; }

        //public override string ToString()
        // {
        //   return $"{Degree} {FirstName} {LastName} ({Rate} ставки)";
        //}
 *             //Console.WriteLine("Writing With XmlTextWriter");
 *             //Console.WriteLine(Serializer.ToString());
            EmployeesList List = new EmployeesList(); //we create new list which contains all empoyees
            Console.WriteLine(string.Join("\n", susu.Users));
            Stream filestream = new FileStream(filename, FileMode.Create);
            var StringSerialize = new XmlSerializer(typeof(List<OrganisationBC>));
            XmlWriter StringToXML = new XmlTextWriter(filestream, Encoding.Unicode);
            StringSerialize.Serialize(StringToXML, List.Employees); //perf

            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(writer.ToString());
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
            

            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");
            using (var writerr = File.CreateText(backingFile))
            {
                writerr.WriteLineAsync(StringToXML.ToString());
            }
            

using System;

namespace lab1
{
public class OrganisationBC
{
public static void CalcPayments(char type, )
{

}
static string str;
static bool cout(string inc)
{
Console.WriteLine(inc);
return (true);
}
static string cin()
{
str = Console.ReadLine();
return (str);
}
static void Main(string[] args)
{
if (cout(cin()) == true)
{
    cout("if succeeded!");
}
} 
}
public class Fixed_rate :  OrganisationBC
{

}
public class Hourly_rate : OrganisationBC
{

}
}
*/

