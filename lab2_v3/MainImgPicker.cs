using System;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using CsQuery;

namespace lab2
{
    internal class ImgPicker
    {
        public delegate void ImgFinder(string URI, int Depth, string src, string alt);
        public event ImgFinder Found;
        private int Depth { get; set; }
        public void Parsing(string URI, int depth)
        {
            if (depth >= 0)
            {
                WebClient webclient = new WebClient();
                Uri uri = new Uri(URI);
                string page = String.Empty;
                try { page = webclient.DownloadString(uri); }
                catch (WebException ex) { if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response is HttpWebResponse response) Console.WriteLine("HTTP Status Code: " + (int)response.StatusCode); }
                CQ csquery = CQ.Create(page);
                foreach (IDomObject img in csquery.Find("img")) { Found?.Invoke(URI, Depth, img.GetAttribute("src"), img.GetAttribute("alt")); }
                if (Depth > 0)
                {
                    Regex hrefregex = new Regex("(href\\s*=\\s*(?:\"|')(.*?)(?:\"|'))", RegexOptions.IgnoreCase);
                    Match hrefmatch;
                    hrefmatch = hrefregex.Match(page);
                    while (hrefmatch.Success)
                    {
                        string href = hrefmatch.Value.Substring(5).Trim('"', ' ');
                        if (href.StartsWith("/")) href = uri.GetLeftPart(UriPartial.Authority) + href;
                        if (href.Contains(uri.GetLeftPart(UriPartial.Authority)) && href != URI) Parsing(href, Depth--);
                        hrefmatch = hrefmatch.NextMatch();
                    }
                }
            }
        }
    }

    class MainImgPicker
    {
        private static int depth;
        private static string filename = String.Empty;
        static void Main()
        {
            ImgPicker ipicker = new ImgPicker();
            Console.WriteLine("URI:\n");
            string URI = Console.ReadLine();
            Console.WriteLine("Depth:\n");
            depth = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Save to file? y/n:\n", 'y', 'n');
            if (Console.ReadLine() == "y")
            {
                filename = Console.ReadLine() + ".csv";
                using (StreamWriter file = new StreamWriter(filename, true)) { file.WriteLine("Desription,Link,Depth,URI\n"); }
                Console.WriteLine("File saved: \"" + filename + "\"");
                ipicker.Found += save;
                ipicker.Found += show;
            }
            else ipicker.Found += show;
            ipicker.Parsing(URI, depth);
        }
        private static void show(string URI, int Depth, string src, string alt)
        {
            Console.WriteLine("Description: " + alt);
            Console.WriteLine("Link: " + src);
            Console.WriteLine("Depth: " + (depth - Depth) + "\n");
            Console.WriteLine("URI: " + URI);
        }
        private static void save(string URI, int Depth, string src, string alt)
        {
            StreamWriter file = new StreamWriter(filename, true);
            file.WriteLine(alt + "," + src + "," + Convert.ToString(Depth) + "," + URI + "\n");
        }
    } 
}