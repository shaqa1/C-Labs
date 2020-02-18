using System;

namespace lab1
{
    public class Program
    {
        static string str;
        static bool cout(string inc)
        {
            Console.WriteLine(inc);
            return (true);
        }
        static string cin()
        {
            str = Console.ReadLine();
            return (str);
        }
        static void Main(string[] args)
        {
            if (cout(cin()) == true)
            {
                cout("if succeeded!");
            }
        } 
    }
}
