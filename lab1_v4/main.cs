using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System;

namespace lab1v4
{
    [XmlInclude(typeof(resid))]
    [XmlInclude(typeof(nonresid))]
    public abstract class propertycompany
    {
        public string type { get; set; }
        public string street { get; set; }
        public int house { get; set; }
        public double people { get; set; }
        public static double total_people { get; set; }
        public abstract void calcpeople();
    }
    public class resid : propertycompany
    {
        public int flats { get; set; }
        public int rooms { get; set; }
        public override void calcpeople() { people = flats * rooms * 1.3; }
        public override string ToString() { return string.Format("{0,-17}{1,-22}{2,-15}{3,-6}{4,-13}{5,-6}", type, street, house, flats, rooms, people); }
    }
    public class nonresid : propertycompany
    {
        public double area { get; set; }
        public override void calcpeople() { people = area * 0.2; }
        public override string ToString() { return string.Format("{0,-17}{1,-22}{2,-27}{3,-7}{4,-6}", type, street, house, area, people); }
    }
    internal class main
    {
        public static List<propertycompany> list { get; set; } = new List<propertycompany>();
        private static int Main()
        {
            while (true)
            {
                sort();
            badchoice:
                menu(true);
                ConsoleKeyInfo input = Console.ReadKey();
                if (char.IsDigit(input.KeyChar))
                    switch (int.Parse(input.KeyChar.ToString()))
                    {
                        case 0: return (0);
                        case 1:
                            {
                                head();
                                foreach (propertycompany line in list) Console.WriteLine(line);
                                Console.WriteLine("\nSum of ANoP:" + propertycompany.total_people.ToString().PadLeft(68));
                            }
                            break;
                        case 2:
                            {
                                head();
                                if (list.Count() < 4) for (int y = 0; y < list.Count(); y++) Console.WriteLine(list[y]);
                                else for (int y = 0; y < 3; y++) Console.WriteLine(list[y]);
                            }
                            break;
                        case 3:
                            {
                                draw("Street:               House number:\n");
                                if (list.Count() < 5) for (int y = 0; y < list.Count(); y++) Console.WriteLine("{0,-22}{1, -9}", list[y].street, list[y].house);
                                else for (int y = list.Count() - 4; y < list.Count(); y++) Console.WriteLine("{0,-22}{1, -9}", list[y].street, list[y].house);
                            }
                            break;
                        case 4: add(); break;
                        case 5: serial(); break;
                        case 6: deserial(); break;
                        default: goto badchoice;
                    }
                else goto badchoice;
                menu(false);
            }
        }
        public static void draw(string text) { Console.Clear(); Console.WriteLine(text); }
        public static bool ask(string promt, char a, char b)
        {
            bool decision;
        tryagain:
            draw(promt);
            ConsoleKeyInfo choice = Console.ReadKey();
            if (choice.KeyChar.ToString() == a.ToString()) decision = true;
            else if (choice.KeyChar.ToString() == b.ToString()) decision = false;
            else goto tryagain;
            return decision;
        }
        public static void menu(bool start)
        {
            if (!start) { Console.WriteLine("\nPress any key to return to main menu..."); Console.ReadKey(); }
            draw("1. Show current list\n2. Show first three entries of property list\n3. Show last four addresses of property list\n4. Add a new entry to property list\n5. Export current property list\n6. Import new property list from xml/json file\n0. Exit\n");
        }
        public static void head() { draw("Property type:   Street:               House number:  NoA:  RpA:  Area:  ANoP:\n"); }
        public static void serial()
        {
            draw("Exporting file:\nEnter file path and name followed by extension. Or you can leave file path blank and enter filename only, that will cause file to be created in OS default folder. Both JSON and XML extensions are supported:\n");
        wrongfileoutputextension:
            string filename = Console.ReadLine();
            if (!(filename.ToLower().IndexOf('\\') != -1 || filename.ToLower().IndexOf('/') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            if (filename.Substring(filename.Length - 4) == ".xml" && (Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\') + 1)) || Directory.Exists(filename.Substring(0, filename.LastIndexOf('/') + 1)))) new XmlSerializer(typeof(List<propertycompany>)).Serialize(File.CreateText(filename), list);
            else if (filename.Substring(filename.Length - 5) == ".json" && (Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\') + 1)) || Directory.Exists(filename.Substring(0, filename.LastIndexOf('/') + 1)))) File.WriteAllText(filename, JsonConvert.SerializeObject(list, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
            else { draw("Wrong file path or extension, please specify path correctly and use .json and .xml extensions only. Enter file path and name again:\n"); goto wrongfileoutputextension; }
            draw("File is accessible via path: \"" + filename + "\"");
        }
        public static void deserial()
        {
            if (list.Any() && ask("Current property list is not empty, do you want to save it? Type y/n:", 'y', 'n') == true) serial();
            draw("Importing file:\nEnter file path and name followed by extension. Or you can leave file path blank and enter filename only, path will be considered as OS default folder. Both JSON and XML extensions are supported:\n");
        wrongfileimport:
            string filename = String.Empty;
            while (filename == "") filename = Console.ReadLine();
            if (!(filename.ToLower().IndexOf('\\') != -1 || filename.ToLower().IndexOf('/') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            if (File.Exists(filename))
            {
                Stream filestream = new FileStream(filename, FileMode.Open);
                string streamstring = new StreamReader(filestream).ReadToEnd();
                XmlSerializer XMLSerializer = new XmlSerializer(typeof(List<propertycompany>));
                if (Path.GetExtension(filename) == ".xml" && XMLSerializer.CanDeserialize(new XmlTextReader(filestream)))
                    if (list.Any() && ask("Do you want to merge current list with imported one? Type y/n:", 'y', 'n')) list = list.Concat((List<propertycompany>)XMLSerializer.Deserialize(filestream)).ToList();
                    else list = (List<propertycompany>)XMLSerializer.Deserialize(filestream);
                else if (Path.GetExtension(filename) == ".json" && validjson(streamstring))
                    if (list.Any() && ask("Do you want to merge current list with imported one? Type y/n:", 'y', 'n')) list = list.Concat(JsonConvert.DeserializeObject<List<propertycompany>>(streamstring, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All })).ToList();
                    else list = JsonConvert.DeserializeObject<List<propertycompany>>(streamstring, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                else { draw("Wrong file structure, path, name or extension, please use correct xml or json structure.\nEnter file name again:\n"); filestream.Close(); goto wrongfileimport; }
                filestream.Close();
                draw("File import succeeded.");
            }
            else draw("File doesn't exist!");
        }
        public static bool validjson(string streamstring)
        {
            string JsonSchema = "{ 'type': 'object', 'items': { 'type': [ 'object', 'null' ], 'properties': { 'NumberOfApartments': { 'type': 'integer' }, 'Rooms': { 'type': 'integer' }, 'Type': { 'type': [ 'string', 'null' ] }, 'Street': { 'type': [ 'string', 'null' ] }, 'Building': { 'type': 'integer' }, 'ANoP': { 'type': 'number' }}, 'required': [ 'NumberOfApartments', 'Rooms', 'Type', 'Street', 'Building', 'ANoP'] }}";
            JSchema schema = JSchema.Parse(JsonSchema);
            JObject person = JObject.Parse(streamstring);
            bool valid = person.IsValid(schema, out IList<string> messages);
            Console.ReadKey();
            return valid;
        }
        public static void add()
        {
            if (ask("Adding an entry to property list\nEnter property type: type 'r' for Residential or 'n' for Non-residential:\n", 'r', 'n'))
            {
                draw("Enter Residential property parameters in the following sequence:\nStreet House_number Number_of_Apartments Number_of_Rooms\n");
                string[] fields = Console.ReadLine().Split(' ');
                resid residential = new resid { type = "Residential", street = fields[0], house = Convert.ToInt32(fields[1]), flats = int.Parse(fields[2]), rooms = int.Parse(fields[3]) };
                residential.calcpeople();
                list.Add(residential);
            }
            else
            {
                draw("Enter Non-Residential property parameters in the following sequence:\nStreet House_Number House_Area\n");
                string[] fields = Console.ReadLine().Split(' ');
                nonresid nonresidential = new nonresid { type = "Non-Residential", street = fields[0], house = Convert.ToInt32(fields[1]), area = double.Parse(fields[2]) };
                nonresidential.calcpeople();
                list.Add(nonresidential);
            }
            draw("Entry has been successfully added.");
        }
        public static void sort()
        {
            list = list.OrderBy(o => o.people).ToList();
            propertycompany.total_people = 0;
            foreach (propertycompany listline in list) propertycompany.total_people += listline.people;
        }
    }
}