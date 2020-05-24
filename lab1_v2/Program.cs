using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace lab1
{
    [XmlInclude(typeof(CargoFlight))]
    [XmlInclude(typeof(PassengerFlight))]
    public class Staff
    {
        public string Name { get; set; }
        public string Post { get; set; }
        public override string ToString() { return $" {Name} - {Post}"; }
    }
    public abstract class Aircraft
    {
        public List<Staff> StaffList { get; set; } = new List<Staff>();
        public int FlightNumber { get; set; }
        public int Weight { get; set; }
        public abstract void WeightDefinition();
    }
    public class CargoFlight : Aircraft
    {
        public int CargoWeight { get; set; }
        public override string ToString()
        {
            string staffstr = String.Empty;
            foreach (Staff staff in StaffList) { staffstr += staff.ToString() + "\n"; }
            return $"{FlightNumber} Cargo Flight\n" + $"Staff:\n" + $"{staffstr}" + $"Overall Weight: {Weight}\n";
        }
        public override void WeightDefinition() { Weight = 100000 + CargoWeight; }
        public CargoFlight(int FlightNumber, List<Staff> StaffList, int CargoWeight)
        {
            this.FlightNumber = FlightNumber;
            this.StaffList = StaffList;
            this.CargoWeight = CargoWeight;
            WeightDefinition();
        }
    }
    public class PassengerFlight : Aircraft
    {
        public int Seats { get; set; }
        public override string ToString()
        {
            string staffstr = String.Empty;
            foreach (Staff staff in StaffList) { staffstr += staff.ToString() + "\n"; }
            return $"{FlightNumber} Cargo Flight\n" + $"Staff:\n" + $"{staffstr}" + $"Overall Weight: {Weight}\n";
        }
        public override void WeightDefinition() { Weight = 50000 + Seats * 62; }
        public PassengerFlight(int FlightNumber, List<Staff> StaffList, int Seats)
        {
            this.FlightNumber = FlightNumber;
            this.StaffList = StaffList;
            this.Seats = Seats;
            WeightDefinition();
        }
    }
    internal class Program
    {
        public static double AverageWeight;
        public static List<Aircraft> AircraftList { get; set; } = new List<Aircraft>();
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
                        case 0: return (0);
                        case 1:
                            {
                                Console.Clear();
                                foreach (Aircraft listline in AircraftList) Console.WriteLine(listline);
                            }
                            break;
                        case 2:
                            {
                                Console.Clear();
                                if (AircraftList.Count() < 6) for (int y = 0; y < AircraftList.Count(); y++) Console.WriteLine(AircraftList[y]);
                                else for (int y = 0; y < 6; y++) Console.WriteLine(AircraftList[y]);
                            }
                            break;
                        case 3:
                            {
                                Console.Clear();
                                if (AircraftList.Count() < 2) for (int y = 0; y < AircraftList.Count(); y++) Console.WriteLine(AircraftList[y]);
                                else for (int y = AircraftList.Count() - 2; y < AircraftList.Count(); y++) Console.WriteLine(AircraftList[y]);
                            }
                            break;
                        case 4: AddToList(); break;
                        case 5: SerializeObject(); break;
                        case 6: DeSerializeObject(); break;
                        case 7: ClearDraw($"Overall average weight is {AverageWeight}"); break;
                        default: goto badchoice;
                    }
                else goto badchoice;
                DrawMenu(false);
            }
        }
        public static void DrawMenu(bool start)
        {
            if (!start) { Console.WriteLine("\nPress any key to return to main menu..."); Console.ReadKey(); }
            ClearDraw("1. Show current flights list\n2. Show first six entries of flights list\n3. Show last two entries of flights list\n4. Add a new entry to flights list\n5. Export current flights list\n6. Import new flights list from xml/json file\n7. Show overall average weight\n0. Exit\n");
        }
        public static void SerializeObject() //file export
        {
            ClearDraw("Exporting file:\nEnter file path and name followed by extension. Or you can leave file path blank and enter filename only, that will cause file to be created in OS default folder. Both JSON and XML extensions are supported:\n");
        wrongfileoutputextension:
            string filename = Console.ReadLine();
            if (!(filename.ToLower().IndexOf('\\') != -1 || filename.ToLower().IndexOf('/') != -1)) filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
            if (filename.Substring(filename.Length - 4) == ".xml" && (Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\') + 1)) || Directory.Exists(filename.Substring(0, filename.LastIndexOf('/') + 1)))) new XmlSerializer(typeof(List<Aircraft>)).Serialize(File.CreateText(filename), AircraftList);
            else if (filename.Substring(filename.Length - 5) == ".json" && (Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\') + 1)) || Directory.Exists(filename.Substring(0, filename.LastIndexOf('/') + 1)))) File.WriteAllText(filename, JsonConvert.SerializeObject(AircraftList, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
            else { ClearDraw("Wrong file path or extension, please specify path correctly and use .json and .xml extensions only. Enter file path and name again:\n"); goto wrongfileoutputextension; }
            ClearDraw("File is accessible via path: \"" + filename + "\"");
        }
        public static void DeSerializeObject()
        {
            if (AircraftList.Any() && Promt("Current property list is not empty, do you want to save it? Type y/n:", 'y', 'n') == true) SerializeObject();
            ClearDraw("Importing file:\nEnter file path and name followed by extension. Or you can leave file path blank and enter filename only, path will be considered as OS default folder. Both JSON and XML extensions are supported:\n");
        wrongfileimport:
            string filename = String.Empty;
            while (filename == "") filename = Console.ReadLine();
            if (!(filename.ToLower().IndexOf('\\') != -1 || filename.ToLower().IndexOf('/') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            if (File.Exists(filename))
            {
                Stream filestream = new FileStream(filename, FileMode.Open);
                string streamstring = new StreamReader(filestream).ReadToEnd();
                XmlSerializer XMLSerializer = new XmlSerializer(typeof(List<Aircraft>));
                if (Path.GetExtension(filename) == ".xml" && XMLSerializer.CanDeserialize(new XmlTextReader(filestream)))
                    if (AircraftList.Any() && Promt("Do you want to merge current list with imported one? Type y/n:", 'y', 'n')) AircraftList = AircraftList.Concat((List<Aircraft>)XMLSerializer.Deserialize(filestream)).ToList();
                    else AircraftList = (List<Aircraft>)XMLSerializer.Deserialize(filestream);

                else if (Path.GetExtension(filename) == ".json")
                    if (AircraftList.Any() && Promt("Do you want to merge current list with imported one? Type y/n:", 'y', 'n')) AircraftList = AircraftList.Concat(JsonConvert.DeserializeObject<List<Aircraft>>(streamstring, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All })).ToList();
                    else AircraftList = JsonConvert.DeserializeObject<List<Aircraft>>(streamstring, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                else { ClearDraw("Wrong file structure, path, name or extension, please use correct xml or json structure.\nEnter file name again:\n"); filestream.Close(); goto wrongfileimport; }
                filestream.Close();
                ClearDraw("File import succeeded.");
            }
            else ClearDraw("File doesn't exist!");
        }
        public static void AddToList()
        {
            if (Promt("Adding an entry to flight list\nEnter flight type: type 'c' for Cargo flight or 'p' for Passenger flight:\n", 'c', 'p'))
            {
                ClearDraw("Enter cargo flight parameters in the following sequence:\nFlight_Number Cargo_Weight\n");
                string[] flightfields = Console.ReadLine().Split(' ');
                List<Staff> LocalStaffList = new List<Staff>();
            staffentry:
                ClearDraw("Enter staff details in the following sequence:\nName Post\n");
                string[] stafffields = Console.ReadLine().Split(' ');
                LocalStaffList.Add(new Staff { Name = stafffields[0], Post = stafffields[1] });
                if (Promt("Want to add one more staff person? y/n", 'y', 'n')) goto staffentry;
                CargoFlight Aircraft = new CargoFlight(Convert.ToInt32(flightfields[0]), LocalStaffList, Convert.ToInt32(flightfields[1]));
                AircraftList.Add(Aircraft);
                RefreshAverageWeight();
            }
            else
            {
                ClearDraw("Enter cargo flight parameters in the following sequence:\nFlight_Number Seats_Amount\n");
                string[] flightfields = Console.ReadLine().Split(' ');
                List<Staff> LocalStaffList = new List<Staff>();
            staffentry:
                ClearDraw("Enter staff details in the following sequence:\nName Post\n");
                string[] stafffields = Console.ReadLine().Split(' ');
                LocalStaffList.Add(new Staff { Name = stafffields[0], Post = stafffields[1] });
                if (Promt("Want to add one more staff person? y/n", 'y', 'n')) goto staffentry;
                PassengerFlight Aircraft = new PassengerFlight(Convert.ToInt32(flightfields[0]), LocalStaffList, Convert.ToInt32(flightfields[1]));
                AircraftList.Add(Aircraft);
                RefreshAverageWeight();
            }
            ClearDraw("Entry has been successfully added.");
        }
        public static void SortList() { AircraftList = AircraftList.OrderBy(obj => obj.FlightNumber).ToList(); }
        public static void RefreshAverageWeight()
        {
            AverageWeight = 0;
            foreach (Aircraft plane in AircraftList) { AverageWeight += plane.Weight; }
            AverageWeight /= AircraftList.Count;
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
    }
}