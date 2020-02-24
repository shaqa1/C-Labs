using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace lab1
{
    [XmlInclude(typeof(Residential))]
    [XmlInclude(typeof(NonResidential))]
    public abstract class ManagementCompanyBC //abstract means that it's just a base class, it can contain abstract methods
    {
        public string Type { get; set; }
        public string Building { get; set; }
        public string Street { get; set; }
        public double ANoP { get; set; }
        public abstract void AverageNumberOfPeople();    
    }
    public class Residential : ManagementCompanyBC //derived from abstract class mancompbc
    {
        public int NumberOfApartments { get; set; }
        public double Rooms { get; set; }
        public override void AverageNumberOfPeople()
        {
            ANoP = NumberOfApartments * Rooms * 1.3;
        }
        public override string ToString() //overrides ToString() method in order to output string that contains multiple vars
        {
            return string.Format("{0,-17}{1,-22}{2,-15}{3,-6}{4,-13}{5,-6}", Type, Street, Building, NumberOfApartments, Rooms, ANoP);
        }
    }
    public class NonResidential : ManagementCompanyBC
    {
        public double Area { get; set; }
        public override void AverageNumberOfPeople()
        {
            ANoP = Area * 0.2;
        }
        public override string ToString() //overrides ToString() method in order to output string that contains multiple vars
        {
            return string.Format("{0,-17}{1,-22}{2,-27}{3,-7}{4,-6}", Type, Street, Building, Area, ANoP);
        }
    }
    public class CreatePropertyList
    {
        public List<ManagementCompanyBC> PropertyList { get; set; } = new List<ManagementCompanyBC>();
    }
    internal class Program
    {
        public static CreatePropertyList List = new CreatePropertyList();
        private static void Main()
        {
            while (true)
            {
                SortList();
            badchoice:
                DrawMenu(true);
                var i = Console.ReadLine();
                switch (i)
                {
                    case "0": break;//return (0);
                    case "1":
                        {
                            Console.Clear();
                            Console.WriteLine("Property type:   Street:               House number:  NoA:  RpA:  Area:  ANoP:\n");
                            foreach (ManagementCompanyBC listline in List.PropertyList) //this will cause overrided ToString() method to be executed
                            {
                                Console.WriteLine(listline);
                            }
                            DrawMenu(false);
                        }
                        break;
                    case "2":
                        {
                            Console.Clear();
                            Console.WriteLine("Property type:   Street:               House number:  NoA:  RpA:  Area:  ANoP:\n");
                            if (List.PropertyList.Count() < 4) for (int y = 0; y < List.PropertyList.Count(); y++) Console.WriteLine(List.PropertyList[y]);
                            else for (int y = 0; y < 3; y++) Console.WriteLine(List.PropertyList[y]);
                            DrawMenu(false);
                        }
                        break;
                    case "3":
                        {
                            Console.Clear();
                            Console.WriteLine("Street:               House number:\n");
                            if (List.PropertyList.Count() < 5) for (int y = 0; y < List.PropertyList.Count(); y++) Console.WriteLine("{0,-22}{1, -9}", List.PropertyList[y].Street, List.PropertyList[y].Building);
                            else for (int y = List.PropertyList.Count() - 4; y < List.PropertyList.Count(); y++) Console.WriteLine("{0,-22}{1, -9}", List.PropertyList[y].Street, List.PropertyList[y].Building);
                            DrawMenu(false);
                        }
                        break;
                    case "4":
                        { AddToList(); DrawMenu(false); }
                        break;
                    case "5":
                        { SerializeObject(); DrawMenu(false); }
                        break;
                    case "6":
                        { DeSerializeObject(); DrawMenu(false); }
                        break;
                    default: goto badchoice;
                }
            }
        }
        public static void SerializeObject() //file export
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
                using StreamWriter filestream = File.CreateText(filename);
                XmlSerializer XMLSerializer = new XmlSerializer(typeof(List<ManagementCompanyBC>));
                XMLSerializer.Serialize(filestream, List.PropertyList);
            }
            /*THIS METHOD OF SERIALIZING TO JSON IN UNSAFE AND SHOULD NOT BE USED IN REAL LIFE APPLICATIONS*/
            else if (filename.Substring(filename.Length - 5) == ".json")
            {
                var JSONSerializerOptions = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                File.WriteAllText(filename, JsonConvert.SerializeObject(List.PropertyList, JSONSerializerOptions));
            }
            /*END OF DANGEROUS METHOD*/
            else
            {
                Console.WriteLine("Wrong file extension, please use .json and .xml only. Enter file name:\n");
                goto wrongfileoutputextension;
            }
            Console.WriteLine("\nFile is accessible via path: \"" + filename + "\"");
        }
        public static void DeSerializeObject() //file import
        {
            Console.Clear();
            if (List.PropertyList.Any()) //check if list is not empty and promt to save current configuration
            {
                Console.WriteLine("Current property list is not empty, do you want to save it? Type Y/N:");
                if (Console.ReadLine() == "y" || Console.ReadLine() == "Y")
                {
                    SerializeObject();
                    Console.WriteLine("Successfully saved.");
                }
                else Console.Clear();
            }
            Console.WriteLine("Enter file path and name followed by extension. Or you can leave file path blank and enter filename only, path will be considered as OS default folder. Both JSON and XML extensions are supported:\n");
        wrongfileinputextension:
            string filename = Console.ReadLine();
            if (!(filename.ToLower().IndexOf('\\') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            if (filename.Substring(filename.Length - 4) == ".xml")
            {
                XmlSerializer XMLSerializer = new XmlSerializer(typeof(List<ManagementCompanyBC>));
                using Stream filestream = new FileStream(filename, FileMode.Open);
                List.PropertyList = (List<ManagementCompanyBC>)XMLSerializer.Deserialize(filestream); //call the Deserialize method to restore the object's state
            }
            else if (filename.Substring(filename.Length - 5) == ".json")
            {
                var JSONDeSerializerOptions = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                List.PropertyList = JsonConvert.DeserializeObject<List<ManagementCompanyBC>>(File.ReadAllText(filename), JSONDeSerializerOptions);
            }
            else
            {
                Console.WriteLine("Wrong file extension, please use .json and .xml files only. Enter file name:\n");
                goto wrongfileinputextension;
            }
            Console.WriteLine("\nFile import succeeded.");
        }
        public static void AddToList()
        {
            Console.Clear();
            Console.WriteLine("Adding an entry to property list");
            Console.WriteLine("Enter property type: 'R' for Residential / 'N' for Non-residential\n");
            string choice = Console.ReadLine();
            if (choice == "R" || choice == "r")
            {
                Console.Clear();
                Console.WriteLine("Enter Residential property parameters in the following sequence:\nStreet House_number Number_of_Apartments Number_of_Rooms\n");
                string[] fields = Console.ReadLine().Split(' ');
                Residential residential = new Residential { Type = "Residential", Street = fields[0], Building = fields[1], NumberOfApartments = int.Parse(fields[2]), Rooms = int.Parse(fields[3]) };
                residential.AverageNumberOfPeople();
                Program.List.PropertyList.Add(residential);
                Console.WriteLine("Successfully added.");
            }
            else if (choice == "N" || choice == "n")
            {
                Console.Clear();
                Console.WriteLine("Enter Non-Residential property parameters in the following sequence:\nStreet House_Number House_Area\n");
                string[] fields = Console.ReadLine().Split(' ');
                NonResidential nonresidential = new NonResidential { Type = "Non-Residential", Street = fields[0], Building = fields[1], Area = double.Parse(fields[2]) };
                nonresidential.AverageNumberOfPeople();
                List.PropertyList.Add(nonresidential);
                Console.WriteLine("Successfully added.");
            }
            else
            {
                Console.Clear(); Console.WriteLine("Wrong choice!");
            }
        }
        public static void SortList()
        {
            List.PropertyList = List.PropertyList.OrderBy(o => o.ANoP).ToList();
        }
        public static void DrawMenu(bool start)
        {
            if (start) Console.Clear();
            else
            {
                Console.WriteLine("\nPress any key to get back to main menu...");
                Console.ReadKey();
                Console.Clear();
            }
            Console.WriteLine("1. Show current list\n2. Show xxx files\n3. Show xxx files\n4. Add a new entry to property list\n5. Export current property list\n6. Import new property list from file\n0. Exit\n");
        }
    }
}
/