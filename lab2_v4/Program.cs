using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace lab2_v4
{
    internal class EmailPicker
    {
        public delegate void Analyzer(string URI, int Depth, string Email);
        public event Analyzer AnalyzerEvent;
        private void Analyze(string URI, int Depth)
        {
            try
            {
                WebClient client = new WebClient();
                Uri uri = new Uri(URI);
                string pagestring = client.DownloadString(uri);
                Regex emailregex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.IgnoreCase);
                Match emailmatch = emailregex.Match(pagestring);
                while (emailmatch.Success)
                {
                    AnalyzerEvent?.Invoke(URI, Depth, emailmatch.Value);
                    emailmatch = emailmatch.NextMatch();
                }
                if (Depth > 0)
                {
                    Regex hrefregex = new Regex("(href\\s*=\\s*(?:\"|')(.*?)(?:\"|'))", RegexOptions.IgnoreCase);
                    Match hrefmatch;
                    hrefmatch = hrefregex.Match(pagestring);
                    while (hrefmatch.Success)
                    {
                        string href = hrefmatch.Value.Substring(5).Trim('"', ' ');
                        if (href.StartsWith("/ru/")) href = uri.GetLeftPart(UriPartial.Authority) + href;
                        if (href.Contains("susu.ru/ru/") && href != URI) Analyze(href, Depth - 1);
                        hrefmatch = hrefmatch.NextMatch();
                    }
                }
            }
            catch {}
        }
        class Program
        {
            private static List<string> EmailList = new List<string>();
            private static int ScanDepth = 0;
            private static string filename = String.Empty;
            static void Main()
            {
                EmailPicker webanalysis = new EmailPicker();
                ClearDraw("Set the URI that should be scanned:\n");
                string URI = Console.ReadLine();
                ClearDraw("Set scanning depth:\n");
                ScanDepth = Convert.ToInt32(Console.ReadLine());
                if (Promt("Do you want the result to be saved? Type 'y' for yes, or 'n' for no:\n", 'y', 'n'))
                {
                    ClearDraw("Exporting file:\nEnter file path and name. Or you can leave file path blank and enter filename only, that will cause file to be created in OS default folder:\n");
                wrongfileoutputextension:
                    filename = Console.ReadLine() + ".csv";
                    if (!(filename.ToLower().IndexOf('\\') != -1 || filename.ToLower().IndexOf('/') != -1)) filename = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
                    if (Directory.Exists(filename.Substring(0, filename.LastIndexOf('\\') + 1)) || Directory.Exists(filename.Substring(0, filename.LastIndexOf('/') + 1)))
                    {
                        FileStream fstream = new FileStream(filename, FileMode.Create);
                        StreamWriter swriter = new StreamWriter(fstream);
                        swriter.Write("E-Mail,Depth,URI\n");
                        swriter.Close();
                        fstream.Close();
                        ClearDraw("File will be accessible via path: \"" + filename + "\"");
                    }
                    else { ClearDraw("Wrong file path, please specify correct path. Enter file path and name again:\n"); goto wrongfileoutputextension; }
                    webanalysis.AnalyzerEvent += Print;
                    webanalysis.AnalyzerEvent += Display;
                }
                else webanalysis.AnalyzerEvent += Display;
                ClearDraw("Press any key to start scanning...\n");
                Console.ReadKey();
                webanalysis.Analyze(URI, ScanDepth);
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
            private static void Print(string URI, int depth, string Email)
            {
                FileStream fs = new FileStream(filename, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(Email + "," + Convert.ToString(depth) + "," + URI + "\n");
                sw.Close();
                fs.Close();
            }
            private static void Display(string URI, int Depth, string Email)
            {
                if (!EmailList.Contains(Email))
                {
                    EmailList.Add(Email);
                    Console.WriteLine("E-Mail: " + Email);
                    Console.WriteLine("Uri: " + URI);
                    Console.WriteLine("Depth: " + (ScanDepth - Depth) + "\n");
                }
            }
        }
    }
}