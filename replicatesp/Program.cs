using System;
using System.Drawing;

namespace replicatesp
{
    internal class Program
    {
        static int versionMajor = 1;
        static int versionMinor = 0;
        static int versionRevision = 0;

        static void Main(string[] args)
        {
            //Arguments.Usage();
            Print.Write(ConsoleColor.Magenta, "Connecting [");
            new Spinner(Symbols.SpinnerType.Braily, ConsoleColor.Magenta, 0, -1);
            Print.Write(ConsoleColor.Magenta, " ]");
            Print.NewLine();
            Console.ReadKey();
            Print.Write(ConsoleColor.Cyan, "Connecting [");
            new Spinner(Symbols.SpinnerType.Bars, ConsoleColor.Cyan);
            Print.Write(ConsoleColor.Cyan, " ]");


        }
    }
}
