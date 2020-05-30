using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace lab1
{
    [XmlInclude(typeof(Square))]
    [XmlInclude(typeof(Rectangle))]
    [XmlInclude(typeof(Circle))]
    [XmlInclude(typeof(Ellips))]
    
    public abstract class Figure
    {
        public string figure { get; set; }
        public double area { get; set; }
        public uint thickness { get; set; }
        public abstract void getarea();
        public abstract int drawfigure(int currentPos);
    }

    public class Rectangle : Figure
    {
        public double A;
        public double B;
        public Rectangle()
        {
            figure = "rectangle";
            A = B = 0;
            thickness = 0;
        }
        public Rectangle(double a,double b)
        {
            figure = "rectangle";
            A = a;
            B = b;
            thickness = 0;
            getarea();
        }
        public Rectangle(double a,double b,uint frame)
        {
            figure = "rectangle";
            A = a;
            B = b;
            thickness = frame;
            getarea();
        }
        public override void getarea() { area = (A + thickness) * (B + thickness); }
        public override string ToString() { return $"Type of figure:{figure}; Frame thickness:{thickness}; Area:{area};"; }
        public override int drawfigure(int currentPos)
        {
            if (A > Console.WindowWidth || B > Console.WindowHeight) { Console.WriteLine("figure is too big"); return 1; }
            else
            {
                for (int i = 0; i < Console.WindowHeight / 2 - B / 2 + currentPos; i++) { Console.WriteLine(); }
                for (int k = 0; k < B; k++)
                {
                    for (int i = 0; i < Console.WindowWidth / 2 - A / 2; i++) { Console.Write(' '); }
                    for (int i = 0; i < A; i++) { Console.Write('*'); }
                    Console.WriteLine();
                }
            }
            return Console.WindowHeight;
        }
    }
    public class Square : Figure
    {
        public double A { get; set; }
        public Square()
        {
            figure = "square";
            A = 0;
            thickness = 0;
        }
        public Square(double a)
        {
            figure = "square";
            A = a;
            thickness = 0;
            getarea();
        }
        public Square(double a, uint frame)
        {
            figure = "square";
            A = a;
            thickness = frame;
            getarea();
        }
        public override void getarea() { area = (A + thickness) * (A + thickness); }
        public override string ToString() { return $"Type of figure:{figure}; Frame thickness:{thickness}; Area:{area};"; }
        public override int drawfigure(int currentPos)
        {
            if (A > Console.WindowWidth || A > Console.WindowHeight) { Console.WriteLine("figure is too big for console"); return 1; }
            else
            {
                for (int i = 0; i < Console.WindowHeight / 2 - A / 2 + currentPos; i++) { Console.WriteLine(); }
                for (int k = 0; k < A; k++)
                {
                    for (int i = 0; i < Console.WindowWidth / 2 - A / 2; i++) { Console.Write(' '); }
                    for (int i = 0; i < A; i++) { Console.Write('*'); }
                    Console.WriteLine();
                }
            }
            return Console.WindowHeight;
        }
    }
    public class Circle : Figure
    {
        public double Radius;
        public Circle()
        {
            figure = "circle";
            thickness = 0;
            Radius = 0;
        }
        public Circle(double radius)
        {
            figure = "circle";
            thickness = 0;
            Radius = radius;
            getarea();
        }
        public Circle(double radius, uint frame)
        {
            figure = "circle";
            thickness = frame;
            Radius = radius;
            getarea();
        }
        public override void getarea() { area = 3.14 * Math.Pow(Radius + thickness, 2); }
        public override string ToString() { return $"Type of figure:{figure}; Frame thickness:{thickness}; Area:{area}"; }
        public override int drawfigure(int currentPos)
        {
            Action<int, int> write = (xp, yp) => { Console.SetCursorPosition(xp, yp); Console.Write("*"); };
            int centerX = 40, centerY = 12+currentPos, radius = Convert.ToInt32(Radius), x = -radius;
            while (x <= radius)
            {
                var y = (int)Math.Floor(Math.Sqrt(radius * radius - x * x));
                write(x + centerX, y + centerY);
                y = -y;
                write(x + centerX, y + centerY);
                x++;
            }
            Console.SetCursorPosition(0, 25 + currentPos);
            return Console.WindowHeight;
        }
    }

    public class Ellips : Figure
    {
        public double Major_Radius { get; set; }
        public double Minor_Radius { get; set; }
        public Ellips()
        {
            figure = "ellipsis";
            Major_Radius = Minor_Radius = 0;
            thickness = 0;
        }
        public Ellips(double major_radius,double minor_radius)
        {
            figure = "ellipsis";
            Major_Radius = major_radius;
            Minor_Radius = minor_radius;
            thickness = 0;
            getarea();
        }
        public Ellips(double major_radius, double minor_radius,uint frame)
        {
            figure = "ellipsis";
            Major_Radius = major_radius;
            Minor_Radius = minor_radius;
            thickness = frame;
            getarea();
        }
        public override void getarea() { area = 3.14 * (Major_Radius + thickness) * (Minor_Radius + thickness); }
        public override string ToString() { return $"Type of figure:{figure}; Frame thickness:{thickness}; Area:{area}"; }
        public override int drawfigure(int currentPos)
        {
            double width = Major_Radius * 2,height=Minor_Radius*2;
            if (height > Console.WindowHeight || width > Console.WindowWidth) { Console.WriteLine("figure is too big for console"); return 1; }
            int centerX = 40, centerY = 12 + currentPos;
            for (int i = 0; i < width; i++)
            {
                int dx = Convert.ToInt32(i - width / 2);
                int x = centerX + dx;
                int h = (int)Math.Round(height * Math.Sqrt(width * width / 4.0 - dx * dx) / width);
                for (int dy = 1; dy <= h; dy++)
                {
                    Console.SetCursorPosition(x, centerY + dy);
                    Console.Write('*');
                    Console.SetCursorPosition(x, centerY - dy);
                    Console.Write('*');
                }
                if (h >= 0)
                {
                    Console.SetCursorPosition(x, centerY);
                    Console.Write('*');
                }
                Console.Write('*');
            }
            Console.SetCursorPosition(0, 25 + currentPos);
            return Console.WindowHeight;
        }
    }
    public class Graphics_editor
    {
        public double AvgArea { get; set; }
        public List<Figure> Figures { get; } = new List<Figure>();
        public void Sorting()
        {
            Figures.Sort(delegate (Figure x, Figure y)
            {
                if (x.area == y.area)
                    return x.thickness.CompareTo(y.thickness);
                else
                    return x.area.CompareTo(y.area);
            });
        }
    }
    class Program
    {
        static public void MENU()
        {
            Console.WriteLine("this is menu of programm:");
            Console.WriteLine("1) input a figure");
            Console.WriteLine("2) sort and display");
            Console.WriteLine("3) show the type of the first three figures");
            Console.WriteLine("4) draw last two figures");
            Console.WriteLine("5) serialize list in xml and json");
            Console.WriteLine("0) close");
            Console.WriteLine("!) get a menu");
        }
        static public string ReplaceDotOnComma(string str)
        {
            if(str.Contains("."))
            {
                str = str.Replace(".", ",");
                return str;
            }
            return str;
        }
        static void Main(string[] args)
        {
            Graphics_editor editor = new Graphics_editor { AvgArea = 0.0 };
            MENU();
            Console.Write('>');
            string choice = Console.ReadLine();
            while(true)
            {
                switch (choice)
                {
                    case "1":
                        {
                            Console.Clear();
                            string choice1;
                            Console.WriteLine("1) via console\n2) via xml or json file");
                            Console.Write(">");
                            choice1 = Console.ReadLine();
                            switch(choice1)
                            {
                                case "1":
                                    {
                                        Console.WriteLine("choose type of figure:");
                                        Console.Write("1)square\n2)rectangle\n3)circle\n4)ellipsis\n");
                                        choice1 = Console.ReadLine();
                                        switch (choice1)
                                        {
                                            case "1":
                                                {
                                                    double num_a;
                                                    uint num_frame;
                                                    Console.Write("enter side size: ");
                                                    num_a = Convert.ToDouble(Console.ReadLine());
                                                    Console.Write("enter frame thickness: ");
                                                    num_frame = Convert.ToUInt32(Console.ReadLine());
                                                    editor.Figures.Add(new Square(num_a, num_frame));
                                                    break;
                                                }
                                            case "2":
                                                {
                                                    double num_a, num_b;
                                                    uint num_frame;
                                                    num_a = Convert.ToDouble(Console.ReadLine());
                                                    num_b = Convert.ToDouble(Console.ReadLine());
                                                    num_frame = Convert.ToUInt32(Console.ReadLine());
                                                    editor.Figures.Add(new Rectangle(num_a, num_b, num_frame));
                                                    break;
                                                }
                                            case "3":
                                                {
                                                    double num_rad = Convert.ToDouble(Console.ReadLine());
                                                    uint num_frame = Convert.ToUInt32(Console.ReadLine());
                                                    editor.Figures.Add(new Circle(num_rad, num_frame));
                                                    break;
                                                }
                                            case "4":
                                                {
                                                    double maj_rad = Convert.ToDouble(Console.ReadLine()), min_rad = Convert.ToDouble(Console.ReadLine());
                                                    uint num_frame = Convert.ToUInt32(Console.ReadLine());
                                                    editor.Figures.Add(new Ellips(maj_rad, min_rad, num_frame));
                                                    break;
                                                }
                                        }
                                        for (int i = 0; i < editor.Figures.Count; i++)
                                        {
                                            editor.AvgArea += editor.Figures[i].area;
                                        }
                                        editor.AvgArea /= editor.Figures.Count;
                                        break;
                                    }
                                case "2":
                                    {
                                        Console.Clear();
                                        Console.WriteLine("1) xml\n2) json");
                                        Console.Write(">");
                                        choice1 = Console.ReadLine();
                                        switch(choice1)
                                        {
                                            case "1":
                                                {
                                                    Console.Write("input name of xml file: ");
                                                    string NameOfFile = Console.ReadLine();
                                                    while (!NameOfFile.Contains(".xml"))
                                                    {
                                                        Console.WriteLine("error");
                                                        Console.Write("input name of xml file: ");
                                                        NameOfFile = Console.ReadLine();
                                                    }
                                                    uint tempFrame = 0;
                                                    double temp1 = 0.0, temp2 = 0.0;
                                                    try
                                                    {
                                                        XmlDocument xDoc = new XmlDocument();
                                                        xDoc.Load(NameOfFile);
                                                        XmlElement xRoot = xDoc.DocumentElement;
                                                        foreach (XmlElement xnode in xRoot)
                                                        {
                                                            if (xnode.Attributes.Count > 0)
                                                            {
                                                                XmlNode attr = xnode.Attributes.GetNamedItem("xsi:type");
                                                                if (attr != null)
                                                                {
                                                                    switch (attr.Value)
                                                                    {
                                                                        case "Square":
                                                                            {
                                                                                foreach (XmlNode childnode in xnode.ChildNodes)
                                                                                {
                                                                                    switch (childnode.Name)
                                                                                    {
                                                                                        case "Area":
                                                                                            {
                                                                                                editor.AvgArea += Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "A":
                                                                                            {
                                                                                                temp1 = Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "Frame_thickness":
                                                                                            {
                                                                                                tempFrame = Convert.ToUInt32(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;

                                                                                            }
                                                                                    }
                                                                                }
                                                                                editor.Figures.Add(new Square(temp1, tempFrame));
                                                                                break;
                                                                            }
                                                                        case "Rectangle":
                                                                            {
                                                                                foreach (XmlNode childnode in xnode.ChildNodes)
                                                                                {
                                                                                    switch (childnode.Name)
                                                                                    {
                                                                                        case "Area":
                                                                                            {
                                                                                                editor.AvgArea += Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "Frame_thickness":
                                                                                            {
                                                                                                tempFrame = Convert.ToUInt32(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "A":
                                                                                            {
                                                                                                temp1 = Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "B":
                                                                                            {
                                                                                                temp2 = Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                    }
                                                                                }
                                                                                editor.Figures.Add(new Rectangle(temp1, temp2, tempFrame));
                                                                                break;
                                                                            }
                                                                        case "Ellipsis":
                                                                            {
                                                                                foreach (XmlNode childnode in xnode.ChildNodes)
                                                                                {
                                                                                    switch (childnode.Name)
                                                                                    {
                                                                                        case "Area":
                                                                                            {
                                                                                                editor.AvgArea += Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "Frame_thickness":
                                                                                            {
                                                                                                tempFrame = Convert.ToUInt32(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "Major_Radius":
                                                                                            {
                                                                                                temp1 = Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "Minor_Radius":
                                                                                            {
                                                                                                temp2 = Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                    }
                                                                                }
                                                                                editor.Figures.Add(new Ellips(temp1, temp2, tempFrame));
                                                                                break;
                                                                            }
                                                                        case "Circle":
                                                                            {
                                                                                foreach (XmlNode childnode in xnode.ChildNodes)
                                                                                {
                                                                                    switch (childnode.Name)
                                                                                    {
                                                                                        case "Area":
                                                                                            {
                                                                                                editor.AvgArea += Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "Frame_thickness":
                                                                                            {
                                                                                                tempFrame = Convert.ToUInt32(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                        case "Radius":
                                                                                            {
                                                                                                temp1 = Convert.ToDouble(ReplaceDotOnComma(childnode.InnerText));
                                                                                                break;
                                                                                            }
                                                                                    }
                                                                                }
                                                                                editor.Figures.Add(new Circle(temp1, tempFrame));
                                                                                break;
                                                                            }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        Console.WriteLine("file is empty");
                                                    }
                                                    break;
                                                }
                                            case "2":
                                                {
                                                    Console.Write("input name of json file: ");
                                                    string NameOfFile = Console.ReadLine();
                                                    while (!NameOfFile.Contains(".json"))
                                                    {
                                                        Console.WriteLine("error");
                                                        Console.Write("input name of json file: ");
                                                        NameOfFile = Console.ReadLine();
                                                    }
                                                    int indexStart = 0, indexEnd=0;
                                                    JObject elem;
                                                    try
                                                    {
                                                        string subjson, json = File.ReadAllText(NameOfFile);
                                                        json = json.Trim(new char[] { '[', ']' });
                                                        do
                                                        {
                                                            for (int i = indexStart; i < json.Length; i++)
                                                            {
                                                                if (json[i] == '}')
                                                                {
                                                                    indexEnd = i;
                                                                    break;
                                                                }
                                                            }
                                                            subjson = json.Substring(indexStart, (indexEnd - indexStart) + 1);
                                                            elem = JObject.Parse(subjson);
                                                            switch ((string)elem["Figure_Type"])
                                                            {
                                                                case "rectangle":
                                                                    {
                                                                        editor.Figures.Add(new Rectangle((double)elem["A"], (double)elem["B"], (uint)elem["Frame_thickness"]));
                                                                        editor.AvgArea += (double)elem["Area"];
                                                                        break;
                                                                    }
                                                                case "square":
                                                                    {
                                                                        editor.Figures.Add(new Square((double)elem["A"], (uint)elem["Frame_thickness"]));
                                                                        editor.AvgArea += (double)elem["Area"];
                                                                        break;
                                                                    }
                                                                case "circle":
                                                                    {
                                                                        editor.Figures.Add(new Circle((double)elem["Radius"], (uint)elem["Frame_thickness"]));
                                                                        editor.AvgArea += (double)elem["Area"];
                                                                        break;
                                                                    }
                                                                case "ellipsis":
                                                                    {
                                                                        editor.Figures.Add(new Ellips((double)elem["Major_Radius"], (double)elem["Minor_Radius"], (uint)elem["Frame_thickness"]));
                                                                        editor.AvgArea += (double)elem["Area"];
                                                                        break;
                                                                    }
                                                            }
                                                            indexStart = indexEnd + 2;
                                                            elem = null;
                                                        }
                                                        while (indexStart <= json.Length);
                                                    }
                                                    catch
                                                    {
                                                        Console.WriteLine("file is empty");
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            editor.Sorting();
                            foreach (Figure aFigure in editor.Figures)
                            {
                                Console.WriteLine(aFigure);
                            }
                            Console.WriteLine($"Average area: {editor.AvgArea}");
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            for (int i = 0; i < 3; i++)
                            {
                                Console.WriteLine(editor.Figures[i].figure);
                            }
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            int currentPos = 0;
                            if (editor.Figures.Count <= 1)
                            {
                                if (editor.Figures.Count == 0)
                                {
                                    break;
                                }
                                for (int i = 0; i < editor.Figures.Count; i++)
                                {
                                    currentPos = editor.Figures[i].drawfigure(currentPos);
                                }
                                break;
                            }
                            for (int i = editor.Figures.Count - 1; i >= editor.Figures.Count - 2; i--)
                            {

                                currentPos = editor.Figures[i].drawfigure(currentPos);
                            }
                            break;
                        }
                    case "5":
                        {
                            Console.Clear();
                            string NameOfFile;
                            Console.WriteLine("1) in xml\n2) in json");
                            Console.Write('>');
                            string choice5 = Console.ReadLine();
                            Console.Clear();
                            switch(choice5)
                            {
                                case "1":
                                    {
                                        Console.Write("input name of xml file: ");
                                        NameOfFile = Console.ReadLine();
                                        while (!NameOfFile.Contains(".xml"))
                                        {
                                            Console.WriteLine("error");
                                            Console.Write("input name of xml file: ");
                                            NameOfFile = Console.ReadLine();
                                        }
                                        XmlSerializer serializer = new XmlSerializer(typeof(List<Figure>));
                                        using (FileStream fs = new FileStream(NameOfFile, FileMode.OpenOrCreate))
                                        {
                                            serializer.Serialize(fs, editor.Figures);
                                        }
                                        Console.WriteLine("serialized");
                                        break;
                                    }
                                case "2":
                                    {
                                        Console.Write("input name of json file: ");
                                        NameOfFile = Console.ReadLine();
                                        while(!NameOfFile.Contains(".json"))
                                        {
                                            Console.WriteLine("error");
                                            Console.Write("input name of json file: ");
                                            NameOfFile = Console.ReadLine();
                                        }
                                        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                                        using (StreamWriter sw = new StreamWriter(NameOfFile))
                                        using (JsonWriter writer = new JsonTextWriter(sw))
                                        {
                                            serializer.Serialize(writer, editor.Figures);
                                        }
                                        Console.WriteLine("serialized");
                                        break;
                                    }
                            }
                            break;
                        }
                    case "0":
                        {
                            return;
                        }
                    case "!":
                        {
                            Console.Clear();
                            MENU();
                            break;
                        }
                }
                Console.Write('>');
                choice = Console.ReadLine();
            }
        }
    }
}