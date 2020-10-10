using System;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace lab2_v4
{
    internal class ContactsSearcher
    {
        private delegate void Output(string URI, int Depth, string Phone, string Address);
        private event Output OutputEvent;
        private void SearchForContacts(string URI, int SearchDepth)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(URI);
            string HTMLcode = String.Empty;
            try { HTMLcode = client.DownloadString(uri); }
            catch (WebException ex) { if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response is HttpWebResponse response) Console.WriteLine("HTTP Status Code: " + (int)response.StatusCode); }
            Regex AddressRegex = new Regex(@"(?<=\>)(Адрес.*?)(?=\<)", RegexOptions.IgnoreCase);
            Match AddressMatch = AddressRegex.Match(HTMLcode);
            if (AddressMatch.Success) 
            {
                Regex PhoneRegex = new Regex(@"((\d{1,3}-\d{1,3}-\d{1,3}))([xX]\d{1,4})?", RegexOptions.IgnoreCase);
                Match PhoneMatch = PhoneRegex.Match(HTMLcode);
                while (PhoneMatch.Success)
                {
                    OutputEvent?.Invoke(URI, SearchDepth, PhoneMatch.Value, AddressMatch.Value); //invoking output
                    PhoneMatch = PhoneMatch.NextMatch();
                }
            }
            if (SearchDepth > 0)
            {
                Regex hrefregex = new Regex("(href\\s*=\\s*(?:\"|')(.*?)(?:\"|'))", RegexOptions.IgnoreCase);
                Match hrefmatch;
                hrefmatch = hrefregex.Match(HTMLcode);
                while (hrefmatch.Success)
                {
                    string href = hrefmatch.Value.Substring(5).Trim('"', ' ');
                    if (href.StartsWith("/")) href = uri.GetLeftPart(UriPartial.Authority) + href;
                    if (href.Contains(uri.GetLeftPart(UriPartial.Authority)) && href != URI) SearchForContacts(href, SearchDepth--);
                    hrefmatch = hrefmatch.NextMatch();
                }
            }
        }
        class MainParser
        {
            private static List<string> PhoneList = new List<string>();
            private static int ScanDepth = 0;
            private static string filename = String.Empty;
            static void Main()
            {
                ContactsSearcher infofetcher = new ContactsSearcher();
                ClearDraw("URI of resource to be scanned:\n");
                string URI = Console.ReadLine();
                ClearDraw("Depth of scan:\n");
                ScanDepth = Convert.ToInt32(Console.ReadLine());
                if (Promt("Choose whether fetched info will be saved (y) or not (n):\n", 'y', 'n'))
                {
                    ClearDraw("Exporting file:\nSet the file path and name. Or you can leave file path blank and enter filename only, that will cause file to be created in OS default folder:\n");
                wrongfileoutputextension:
                    filename = Console.ReadLine() + ".csv";
                    if (!(filename.ToLower().IndexOf('\\') != -1 || filename.ToLower().IndexOf('/') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
                    if (Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\') + 1)) || Directory.Exists(filename.Substring(0, filename.LastIndexOf('/') + 1)))
                    {
                        using (StreamWriter file = new StreamWriter(filename, true))
                        {
                            file.WriteLine("Address,Phonermation,Depth,URI\n");
                        }
                        ClearDraw("File will be accessible via path: \"" + filename + "\"");
                    }
                    else { ClearDraw("Wrong file path, please specify correct path. Enter file path and name again:\n"); goto wrongfileoutputextension; }
                    infofetcher.OutputEvent += SavePhone;
                    infofetcher.OutputEvent += ShowPhone;
                }
                else infofetcher.OutputEvent += ShowPhone;
                ClearDraw("Press any key to start scanning...\n");
                Console.ReadKey();
                infofetcher.SearchForContacts(URI, ScanDepth);
                Console.WriteLine("\n\n\nDone scanning, press any key to exit...\n");
                Console.ReadKey();
            }
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
            public static void ClearDraw(string text) { Console.Clear(); Console.WriteLine(text); }
            private static void ShowPhone(string URI, int Depth, string Phone, string Address)
            {
                if (!PhoneList.Contains(Phone))
                {
                    PhoneList.Add(Phone);
                    Console.WriteLine("Address: " + Address);
                    Console.WriteLine("Contact information: " + Phone);
                    Console.WriteLine("Depth: " + (ScanDepth - Depth) + "\n");
                    Console.WriteLine("URI: " + URI);
                }
            }
            private static void SavePhone(string URI, int Depth, string Phone, string Address)
            {
                using StreamWriter file = new StreamWriter(filename, true);
                file.WriteLine(Address + "," + Phone + "," + Convert.ToString(Depth) + "," + URI + "\n");
            }
        }
    }
}