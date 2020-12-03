using System;
using System.Net;
using System.IO;
using CsQuery;

namespace lab4
{
    internal class LinksSearcher
    {
        private delegate void Output(string URI, int Depth, string href, string title);
        private event Output OutputEvent;
        private void SearchForLinks(string URI, int SearchDepth)
        {
            Console.WriteLine(SearchDepth);
            if (SearchDepth >= 0)
            {
                WebClient webclient = new WebClient();
                Uri uri = new Uri(URI);
                string page;
                foreach (IDomObject link in CQ.Create(page = webclient.DownloadString(uri)).Find("a")) 
                {
                    string str = link.GetAttribute("href");
                    if (str != null)
                    {
                        if (str.StartsWith("/")) str = uri.GetLeftPart(UriPartial.Authority) + str;
                        if (str != URI)
                        if (str.Contains(uri.GetLeftPart(UriPartial.Authority)) && SearchDepth > 0) SearchForLinks(str, --SearchDepth);
                        else OutputEvent?.Invoke(str, SearchDepth, link.GetAttribute("href"), link.GetAttribute("title"));
                    }
                }
            }
        }
        public class MainParser
        {
            public int ScanDepth = 0;
            public string filename = String.Empty;
            public static void Main()
            {
                MainParser mainparse = new MainParser();
                mainparse.Start();
            }
            public void Start()
            {
                LinksSearcher infofetcher = new LinksSearcher();
                Console.WriteLine("URI of resource to be scanned:\n");
                string URI = Console.ReadLine();
                Console.WriteLine("Depth of scan:\n");
                ScanDepth = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Choose whether fetched info will be saved (y) or not (n):\n", 'y', 'n');
                ConsoleKeyInfo choice = Console.ReadKey();
                if (choice.KeyChar == 'y')
                {
                    Console.WriteLine("Set the csv file name:");
                    filename = Console.ReadLine();
                    using (StreamWriter file = new StreamWriter(filename, true))
                    {
                        file.WriteLine("HREF,Title,Depth,URI\n");
                    }
                    infofetcher.OutputEvent += SaveLink;
                    infofetcher.OutputEvent += ShowLink;
                }
                else infofetcher.OutputEvent += ShowLink;
                infofetcher.SearchForLinks(URI, ScanDepth);
                Console.WriteLine("\n\n\nDone scanning, press any key to exit...\n");
                Console.ReadKey();
            }
            private void ShowLink(string URI, int Depth, string href, string title) { Console.WriteLine("\nLink: " + href + "\nTitle: " + title + "\nDepth: " + (ScanDepth - Depth) + "\nURI: " + URI); }
            private void SaveLink(string URI, int Depth, string href, string title)
            {
                using StreamWriter file = new StreamWriter(filename, true);
                file.WriteLine(href + "," + title + "," + Convert.ToString(Depth) + "," + URI + "\n");
            }
        }
    }
}