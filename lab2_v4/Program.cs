using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using CsQuery;

namespace lab2_v4
{
    internal class WebAnalysis
    {
        public delegate void Analizer(string URI, int Depth, string Source, string alt);
        public event Analizer Notify;

        private bool Crutch = false;
        private int Depth { get; set; }

        public void Analysis(string URI, int depth)
        {
            if (depth >= 0)
            {
                if (Crutch != true)
                {
                    Depth = 0;
                    Crutch = true;
                }
                else
                {
                    Depth += 1;
                }
                WebClient client = new WebClient();
                string page = client.DownloadString(new Uri($"{URI}"));

                CQ cq = CQ.Create(page);
                foreach (IDomObject obj in cq.Find("img"))
                {
                    Notify?.Invoke(URI, Depth, obj.GetAttribute("src"), obj.GetAttribute("alt"));
                }

                if (depth > 0)
                {
                    foreach (IDomObject obj in cq.Find("a"))
                    {
                        if (obj.GetAttribute("href") != null && obj.GetAttribute("href").Contains(URI) && (obj.GetAttribute("href") != URI))
                        {
                            depth = depth - 1;
                            Analysis(obj.GetAttribute("href"), depth);
                        }
                    }
                }
            }

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int ScanDepth;
            var web = new WebAnalysis();
            Console.Write("Set the URI that should be scanned:\n");
            string URI = Console.ReadLine();
            Console.Clear();
            Console.Write("Set scanning depth:\n");
            ScanDepth = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("Press any key to start scanning...\n");
            Console.ReadKey();
            Console.Clear();
            web.Notify += Display;
            web.Analysis(URI, ScanDepth);

            Console.Clear();
            Console.Write("set depth:");
            ScanDepth = Convert.ToInt32(Console.ReadLine());
            FileStream fs = new FileStream("testing.csv", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write("URI;src;alt;depth\n");
            sw.Close();
            fs.Close();
            web.Notify -= Display;
            web.Notify += Print;
            web.Analysis("https://en.wikipedia.org", ScanDepth);
                        break;
            Console.ReadKey();
        }

        private static void Print(string URI, int depth, string src, string alt)
        {
            string str = URI + ";" + src + ";" + alt + ";" + Convert.ToString(depth) + "\n";
            FileStream fs = new FileStream("testing.csv", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(str);
            sw.Close();
            fs.Close();
        }

        private static void Display(string URI, int Depth, string Source, string alt)
        {
            switch (Depth)
            {
                case 0:
                    {
                        Console.WriteLine($"{URI}:");
                        Console.WriteLine("||" + Source);
                        Console.WriteLine("||" + alt);
                        Console.WriteLine("||" + Depth);
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine($"||{URI}:");
                        Console.WriteLine("|-|" + Source);
                        Console.WriteLine("|-|" + alt);
                        Console.WriteLine("|-|" + Depth);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine($"|-|{URI}:");
                        Console.WriteLine("|--|" + Source);
                        Console.WriteLine("|--|" + alt);
                        Console.WriteLine("|--|" + Depth);
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine($"|--|{URI}:");
                        Console.WriteLine("|---|" + Source);
                        Console.WriteLine("|---|" + alt);
                        Console.WriteLine("|---|" + Depth);
                        break;
                    }
            }
        }
    }
}