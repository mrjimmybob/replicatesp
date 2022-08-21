using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace replicatesp
{
    internal static class Print
    {

     
        public static void Error(string name, string error, string detail)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(error + ": ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\'" + name + "\' ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("(" + detail + ")");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Info(string str1, string str2)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(str1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(str2);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Status(string status)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(status);
            Console.ForegroundColor = ConsoleColor.White;

        }

        public static void Write(ConsoleColor Color, string str)
        {
            var PreColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.Write(str);
            Console.ForegroundColor = PreColor;

        }
        


        public static void WriteLine(ConsoleColor Color, string str)
        {
            Write(Color, str);
            Console.WriteLine("");
        }

        public static void NewLine()
        {
            Console.WriteLine("");
        }

        

    }
}
