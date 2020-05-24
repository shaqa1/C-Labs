using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace lab2_v2
{
    internal class EmailPicker
    {
        public delegate void Analyzer(string URI, int Depth, string Email);
        public event Analyzer AnalyzerEvent;
        private void Analyze(string URI, int Depth)
        {
            try
            {
                string pagestring = null;
                Uri uri = new Uri(URI);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (String.IsNullOrWhiteSpace(response.CharacterSet)) readStream = new StreamReader(receiveStream);
                    else readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    pagestring = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                }

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
            static int Main()
            {
                EmailPicker webanalysis = new EmailPicker();
                Console.WriteLine("Set the URI that should be scanned:\n");
                string URI = Console.ReadLine();
                Console.WriteLine("Set scanning depth:\n");
                ScanDepth = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Do you want the result to be saved? Type 'y' for yes, or 'n' for no:\n");
                if (Console.ReadLine() == "y")
                {
                    Console.WriteLine("Enter file path and name:\n");
                    filename = Console.ReadLine() + ".csv";
                    FileStream fstream = new FileStream(filename, FileMode.Create);
                    StreamWriter swriter = new StreamWriter(fstream);
                    swriter.Write("E-Mail,Depth,URI\n");
                    swriter.Close();
                    fstream.Close();
                    Console.WriteLine("File will be accessible via path: \"" + filename + "\"");
                    webanalysis.AnalyzerEvent += Display;
                    webanalysis.AnalyzerEvent += Print;
                }
                else webanalysis.AnalyzerEvent += Display;
                webanalysis.Analyze(URI, ScanDepth);
                return (0);
            }
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