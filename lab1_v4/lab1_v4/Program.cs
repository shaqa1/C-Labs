using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema.Generation;
using System;

namespace lab1
{
    [XmlInclude(typeof(Residential))]
    [XmlInclude(typeof(NonResidential))]
    public abstract class ManagementCompanyBC
    {
        public string Type { get; set; }
        public string Street { get; set; }
        public int Building { get; set; }
        public double ANoP { get; set; }
        public static double SumOfANoP { get; set; }
        public abstract void AverageNumberOfPeople();
    }
    public class Residential : ManagementCompanyBC
    {
        public int NumberOfApartments { get; set; }
        public int Rooms { get; set; }
        public override void AverageNumberOfPeople() { ANoP = NumberOfApartments * Rooms * 1.3; }
        public override string ToString() { return string.Format("{0,-17}{1,-22}{2,-15}{3,-6}{4,-13}{5,-6}", Type, Street, Building, NumberOfApartments, Rooms, ANoP); }
    }
    public class NonResidential : ManagementCompanyBC
    {
        public double Area { get; set; }
        public override void AverageNumberOfPeople() { ANoP = Area * 0.2; }
        public override string ToString() { return string.Format("{0,-17}{1,-22}{2,-27}{3,-7}{4,-6}", Type, Street, Building, Area, ANoP); }
    }
    //public class CreatePropertyList {  }
    internal class Program
    {
        public static List<ManagementCompanyBC> PropertyList { get; set; } = new List<ManagementCompanyBC>();
        //public static CreatePropertyList List = new CreatePropertyList();
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
                                DrawHeader();
                                foreach (ManagementCompanyBC listline in PropertyList) Console.WriteLine(listline);
                                Console.WriteLine("\nSum of ANoP:" + ManagementCompanyBC.SumOfANoP.ToString().PadLeft(68));
                            }
                            break;
                        case 2:
                            {
                                DrawHeader();
                                if (PropertyList.Count() < 4) for (int y = 0; y < PropertyList.Count(); y++) Console.WriteLine(PropertyList[y]);
                                else for (int y = 0; y < 3; y++) Console.WriteLine(PropertyList[y]);
                            }
                            break;
                        case 3:
                            {
                                ClearDraw("Street:               House number:\n");
                                if (PropertyList.Count() < 5) for (int y = 0; y < PropertyList.Count(); y++) Console.WriteLine("{0,-22}{1, -9}", PropertyList[y].Street, PropertyList[y].Building);
                                else for (int y = PropertyList.Count() - 4; y < PropertyList.Count(); y++) Console.WriteLine("{0,-22}{1, -9}", PropertyList[y].Street, PropertyList[y].Building);
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
            if (!(filename.ToLower().IndexOf('\\') != -1 || filename.ToLower().IndexOf('/') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            if (filename.Substring(filename.Length - 4) == ".xml" && (Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\') + 1)) || Directory.Exists(filename.Substring(0, filename.LastIndexOf('/') + 1)))) new XmlSerializer(typeof(List<ManagementCompanyBC>)).Serialize(File.CreateText(filename), PropertyList);
            else if (filename.Substring(filename.Length - 5) == ".json" && (Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\') + 1)) || Directory.Exists(filename.Substring(0, filename.LastIndexOf('/') + 1)))) File.WriteAllText(filename, JsonConvert.SerializeObject(PropertyList, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
            else { ClearDraw("Wrong file path or extension, please specify path correctly and use .json and .xml extensions only. Enter file path and name again:\n"); goto wrongfileoutputextension; }
            ClearDraw("File is accessible via path: \"" + filename + "\"");
        }
        public static void DeSerializeObject()
        {
            if (PropertyList.Any() && Promt("Current property list is not empty, do you want to save it? Type y/n:", 'y', 'n') == true) SerializeObject();
            ClearDraw("Importing file:\nEnter file path and name followed by extension. Or you can leave file path blank and enter filename only, path will be considered as OS default folder. Both JSON and XML extensions are supported:\n");
        wrongfileimport:
            string filename = String.Empty;
            while (filename == "") filename = Console.ReadLine();
            if (!(filename.ToLower().IndexOf('\\') != -1 || filename.ToLower().IndexOf('/') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            if (File.Exists(filename))
            {
                Stream filestream = new FileStream(filename, FileMode.Open);
                string streamstring = new StreamReader(filestream).ReadToEnd();
                string JsonSchema = "{ 'type': 'array', 'items': { 'type': [ 'object', 'null' ], 'properties': { 'NumberOfApartments': { 'type': 'integer' }, 'Rooms': { 'type': 'integer' }, 'Type': { 'type': [ 'string', 'null' ] }, 'Street': { 'type': [ 'string', 'null' ] }, 'Building': { 'type': 'integer' }, 'ANoP': { 'type': 'number' }}, 'required': [ 'NumberOfApartments', 'Rooms', 'Type', 'Street', 'Building', 'ANoP'] }}";
                //string JSONSchemaStr = "{ 'type': 'array', 'items': { 'type': ['object', 'null'], 'properties': { 'Type': { 'type': ['string', 'null']}, 'Building': { 'type': 'number' }, 'Street': { 'type': ['string', 'null' ] }, 'NumberOfApartments': { 'type': 'number' }, 'Rooms': { 'type': 'number' }, 'Area': { 'type': 'number' }, 'ANoP': { 'type': 'number' }}, 'required': ['Type', 'Building', 'Street', 'NumberOfApartments', 'Rooms', 'Area', 'ANoP'] }}";
                //JSchema JSONSchema = JSchema.Parse(JSONSchemaStr); //new JSchemaGenerator().Generate(typeof(List<ManagementCompanyBC>));
                //JObject JSONPropertyList = JObject.Parse(streamstring);

                XmlSerializer XMLSerializer = new XmlSerializer(typeof(List<ManagementCompanyBC>));

                if (Path.GetExtension(filename) == ".xml" && XMLSerializer.CanDeserialize(new XmlTextReader(filestream)))
                    if (PropertyList.Any() && Promt("Do you want to merge current list with imported one? Type y/n:", 'y', 'n')) PropertyList = PropertyList.Concat((List<ManagementCompanyBC>)XMLSerializer.Deserialize(filestream)).ToList();
                    else PropertyList = (List<ManagementCompanyBC>)XMLSerializer.Deserialize(filestream);

                else if (Path.GetExtension(filename) == ".json" && JsonValidate(streamstring))
                    if (PropertyList.Any() && Promt("Do you want to merge current list with imported one? Type y/n:", 'y', 'n')) PropertyList = PropertyList.Concat(JsonConvert.DeserializeObject<List<ManagementCompanyBC>>(streamstring, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All })).ToList();
                    else PropertyList = JsonConvert.DeserializeObject<List<ManagementCompanyBC>>(streamstring, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                else { ClearDraw("Wrong file structure, path, name or extension, please use correct xml or json structure.\nEnter file name again:\n"); filestream.Close(); goto wrongfileimport; }
                // Console.WriteLine(new JSchemaGenerator().Generate(typeof(List<ManagementCompanyBC>)).ToString());  
                filestream.Close();
                ClearDraw("File import succeeded.");
            }
            else ClearDraw("File doesn't exist!");
        }
        public static bool JsonValidate(string streamstring)
        {
            string JsonSchema = "{ 'type': 'object', 'items': { 'type': [ 'object', 'null' ], 'properties': { 'NumberOfApartments': { 'type': 'integer' }, 'Rooms': { 'type': 'integer' }, 'Type': { 'type': [ 'string', 'null' ] }, 'Street': { 'type': [ 'string', 'null' ] }, 'Building': { 'type': 'integer' }, 'ANoP': { 'type': 'number' }}, 'required': [ 'NumberOfApartments', 'Rooms', 'Type', 'Street', 'Building', 'ANoP'] }}";

            //JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(new JsonTextReader(new StringReader(streamstring)));
            //  validatingReader.Schema = JSchema.Parse(JsonSchema);

            //IList<ValidationError> errors;
            // bool valid = colors.IsValid(schema, out errors);
            //Console.WriteLine(messages.Count);
            //JsonSerializer serializer = new JsonSerializer();
            //PropertyList = JsonConvert.DeserializeObject<List<ManagementCompanyBC>>(validatingReader, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            JSchema schema = JSchema.Parse(JsonSchema);

            JObject person = JObject.Parse(streamstring);

            bool valid = person.IsValid(schema, out IList<string> messages);
            //Console.WriteLine(messages[0]);
            //Console.WriteLine(messages[1]);
            Console.ReadKey();
            return valid;
        }
        public static double SimilarityViaLevenshteinAlgorythm(string SourceString, string TargetString)
        {
            if ((SourceString == null) || (TargetString == null) || (SourceString.Length == 0) || (TargetString.Length == 0)) return 0.0;
            if (SourceString == TargetString) return 1.0;
            int SourceStringLength = SourceString.Length;
            int TargetStringLength = TargetString.Length;
            if (SourceStringLength == 0) return TargetStringLength;
            if (TargetStringLength == 0) return SourceStringLength;
            int[,] distance = new int[SourceStringLength + 1, TargetStringLength + 1];
            for (int i = 0; i <= SourceStringLength; distance[i, 0] = i++) ;
            for (int j = 0; j <= TargetStringLength; distance[0, j] = j++) ;
            for (int i = 1; i <= SourceStringLength; i++) for (int j = 1; j <= TargetStringLength; j++) distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + ((TargetString[j - 1] == SourceString[i - 1]) ? 0 : 1));
            int stepsToSame = distance[SourceStringLength, TargetStringLength];
            return (1.0 - ((double)stepsToSame / (double)Math.Max(SourceString.Length, TargetString.Length)));
        }
        public static void AddToList()
        {
            if (Promt("Adding an entry to property list\nEnter property type: type 'r' for Residential or 'n' for Non-residential:\n", 'r', 'n'))
            {
                ClearDraw("Enter Residential property parameters in the following sequence:\nStreet House_number Number_of_Apartments Number_of_Rooms\n");
                string[] fields = Console.ReadLine().Split(' ');
                Residential residential = new Residential { Type = "Residential", Street = fields[0], Building = Convert.ToInt32(fields[1]), NumberOfApartments = int.Parse(fields[2]), Rooms = int.Parse(fields[3]) };
                residential.AverageNumberOfPeople();
                PropertyList.Add(residential);
            }
            else
            {
                ClearDraw("Enter Non-Residential property parameters in the following sequence:\nStreet House_Number House_Area\n");
                string[] fields = Console.ReadLine().Split(' ');
                NonResidential nonresidential = new NonResidential { Type = "Non-Residential", Street = fields[0], Building = Convert.ToInt32(fields[1]), Area = double.Parse(fields[2]) };
                nonresidential.AverageNumberOfPeople();
                PropertyList.Add(nonresidential);
            }
            ClearDraw("Entry has been successfully added.");
        }
        public static void SortList()
        {
            PropertyList = PropertyList.OrderBy(o => o.ANoP).ToList();
            ManagementCompanyBC.SumOfANoP = 0;
            foreach (ManagementCompanyBC listline in PropertyList) ManagementCompanyBC.SumOfANoP += listline.ANoP;
        }
    }
}