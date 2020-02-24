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
    public abstract class ManagementCompanyBC
    {
        public string Type { get; set; }
        public string Building { get; set; }
        public string Street { get; set; }
        public double ANoP { get; set; }
        public static double SumOfANoP { get; set; }
        public abstract void AverageNumberOfPeople();    
    }
    public class Residential : ManagementCompanyBC
    {
        public int NumberOfApartments { get; set; }
        public double Rooms { get; set; }
        public override void AverageNumberOfPeople() { ANoP = NumberOfApartments * Rooms * 1.3; }
        public override string ToString() { return string.Format("{0,-17}{1,-22}{2,-15}{3,-6}{4,-13}{5,-6}", Type, Street, Building, NumberOfApartments, Rooms, ANoP); }
    }
    public class NonResidential : ManagementCompanyBC
    {
        public double Area { get; set; }
        public override void AverageNumberOfPeople() { ANoP = Area * 0.2; }
        public override string ToString() { return string.Format("{0,-17}{1,-22}{2,-27}{3,-7}{4,-6}", Type, Street, Building, Area, ANoP); }
    }
    public class CreatePropertyList { public List<ManagementCompanyBC> PropertyList { get; set; } = new List<ManagementCompanyBC>(); }
    internal class Program
    {
        public static CreatePropertyList List = new CreatePropertyList();
        private static int Main()
        {
            while (true)
            {
                SortList();
            badchoice:
                DrawMenu(true);
                ConsoleKeyInfo input = Console.ReadKey();
                if (char.IsDigit(input.KeyChar))
                switch (int.Parse(input.KeyChar.ToString()))
                {
                    case 0 : return (0);
                    case 1 :
                        {
                            DrawHeader();
                            foreach (ManagementCompanyBC listline in List.PropertyList) Console.WriteLine(listline);
                            Console.WriteLine("\nSum of ANoP:" + ManagementCompanyBC.SumOfANoP.ToString().PadLeft(68));
                        }
                        break;
                    case 2 : 
                        { 
                            DrawHeader(); 
                            if (List.PropertyList.Count() < 4) for (int y = 0; y < List.PropertyList.Count(); y++) Console.WriteLine(List.PropertyList[y]);
                            else for (int y = 0; y < 3; y++) Console.WriteLine(List.PropertyList[y]);
                        }
                        break;
                    case 3 :
                        {
                            ClearDraw("Street:               House number:\n");
                            if (List.PropertyList.Count() < 5) for (int y = 0; y < List.PropertyList.Count(); y++) Console.WriteLine("{0,-22}{1, -9}", List.PropertyList[y].Street, List.PropertyList[y].Building);
                            else for (int y = List.PropertyList.Count() - 4; y < List.PropertyList.Count(); y++) Console.WriteLine("{0,-22}{1, -9}", List.PropertyList[y].Street, List.PropertyList[y].Building);
                        }
                        break;
                    case 4: AddToList(); break;
                    case 5: SerializeObject(); break;
                    case 6: DeSerializeObject(); break;
                    default: goto badchoice;
                }
                else goto badchoice;
                DrawMenu(false);
            }
        }
        public static void ClearDraw(string text) { Console.Clear(); Console.WriteLine(text); }
        public static bool Promt(string promt, char a, char b)
        {
            bool decision;
        tryagain:
            ClearDraw(promt);
            ConsoleKeyInfo choice = Console.ReadKey();
            if (choice.KeyChar.ToString() == a.ToString()) decision = true;
            else if (choice.KeyChar.ToString() == b.ToString()) decision = false;
            else goto tryagain;
            return decision;
        }
        public static void DrawMenu(bool start)
        {
            if (!start) { Console.WriteLine("\nPress any key to return to main menu..."); Console.ReadKey(); }
            ClearDraw("1. Show current list\n2. Show first three entries of property list\n3. Show last four addresses of property list\n4. Add a new entry to property list\n5. Export current property list\n6. Import new property list from xml/json file\n0. Exit\n");
        }
        public static void DrawHeader() { ClearDraw("Property type:   Street:               House number:  NoA:  RpA:  Area:  ANoP:\n"); }
        public static void SerializeObject() //file export
        {
            ClearDraw("Exporting file:\nEnter file path and name followed by extension. Or you can leave file path blank and enter filename only, that will cause file to be created in OS default folder. Both JSON and XML extensions are supported:\n");
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
            else if (filename.Substring(filename.Length - 5) == ".json")
            {
                var JSONSerializerOptions = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                File.WriteAllText(filename, JsonConvert.SerializeObject(List.PropertyList, JSONSerializerOptions));
            }
            else
            {
                ClearDraw("Wrong file extension, please use .json and .xml only. Enter file name again:\n");
                goto wrongfileoutputextension;
            }
            ClearDraw("\nFile is accessible via path: \"" + filename + "\"");
        }
        public static void DeSerializeObject()
        {
            if (List.PropertyList.Any() && Promt("Current property list is not empty, do you want to save it? Type y/n:", 'y', 'n') == true) SerializeObject();
            ClearDraw("Importing file:\nEnter file path and name followed by extension. Or you can leave file path blank and enter filename only, path will be considered as OS default folder. Both JSON and XML extensions are supported:\n");
        wrongfileinputextension:
            string filename = Console.ReadLine();
            if (!(filename.ToLower().IndexOf('\\') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            if (File.Exists(filename) && filename.Substring(filename.Length - 4) == ".xml") if (List.PropertyList.Any() && Promt("Do you want to merge current list with imported one? Type y/n:", 'y', 'n')) List.PropertyList = List.PropertyList.Concat((List<ManagementCompanyBC>)new XmlSerializer(typeof(List<ManagementCompanyBC>)).Deserialize(new FileStream(filename, FileMode.Open))).ToList(); 
                else List.PropertyList = (List<ManagementCompanyBC>)new XmlSerializer(typeof(List<ManagementCompanyBC>)).Deserialize(new FileStream(filename, FileMode.Open));
            else if (File.Exists(filename) && filename.Substring(filename.Length - 5) == ".json") if (List.PropertyList.Any() && Promt("Do you want to merge current list with imported one? Type y/n:", 'y', 'n')) List.PropertyList = List.PropertyList.Concat(JsonConvert.DeserializeObject<List<ManagementCompanyBC>>(File.ReadAllText(filename), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All })).ToList();
                else List.PropertyList = JsonConvert.DeserializeObject<List<ManagementCompanyBC>>(File.ReadAllText(filename), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            else { ClearDraw("\nWrong file path, name or extension, please use .json and .xml files only. Enter file name again:\n"); goto wrongfileinputextension; }
            ClearDraw("File import succeeded.");
        }
        public static void AddToList()
        {
            if (Promt("Adding an entry to property list\nEnter property type: type 'r' for Residential or 'n' for Non-residential:\n", 'r', 'n'))
            {
                ClearDraw("Enter Residential property parameters in the following sequence:\nStreet House_number Number_of_Apartments Number_of_Rooms\n");
                string[] fields = Console.ReadLine().Split(' ');
                Residential residential = new Residential { Type = "Residential", Street = fields[0], Building = fields[1], NumberOfApartments = int.Parse(fields[2]), Rooms = int.Parse(fields[3]) };
                residential.AverageNumberOfPeople();
                Program.List.PropertyList.Add(residential);
            }
            else
            {
                ClearDraw("Enter Non-Residential property parameters in the following sequence:\nStreet House_Number House_Area\n");
                string[] fields = Console.ReadLine().Split(' ');
                NonResidential nonresidential = new NonResidential { Type = "Non-Residential", Street = fields[0], Building = fields[1], Area = double.Parse(fields[2]) };
                nonresidential.AverageNumberOfPeople();
                List.PropertyList.Add(nonresidential);
            }
            ClearDraw("Entry has been successfully added.");
        }
        public static void SortList()
        {
            List.PropertyList = List.PropertyList.OrderBy(o => o.ANoP).ToList();
            foreach (ManagementCompanyBC listline in List.PropertyList) ManagementCompanyBC.SumOfANoP += listline.ANoP;
        }
    }
}