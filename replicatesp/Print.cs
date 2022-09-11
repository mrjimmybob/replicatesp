using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace replicatesp
{
    internal static class Print
    {
        // CmdLineOptions
        static private readonly ConsoleColor programColor = ConsoleColor.Green;
        static private readonly ConsoleColor valueColor = ConsoleColor.Cyan;
        static private readonly ConsoleColor optionColor = ConsoleColor.White;
        static private readonly ConsoleColor separatorColor = ConsoleColor.DarkGray;
        static private readonly ConsoleColor helpColor = ConsoleColor.Green;
        // General messages
        static private readonly ConsoleColor errorColor = ConsoleColor.Red;
        static private readonly ConsoleColor detailColor = ConsoleColor.DarkRed;
        static private readonly ConsoleColor statusColor = ConsoleColor.Magenta;
        static private readonly ConsoleColor infoColor = ConsoleColor.Yellow;
        static private readonly ConsoleColor questionColor = ConsoleColor.Yellow;
        static private readonly ConsoleColor messageColor = ConsoleColor.Cyan;

        public static void Error(string text, string title, string detail = null)
        {
            Write(errorColor, title + ": ");
            Write(messageColor, "\'" + text + "\' ");
            if (detail != null)
                Write(detailColor, "(" + detail + ")");
            NewLine();
        }

        public static void Info(string str1, string str2)
        {
            Write(infoColor, str1);
            WriteLine(messageColor, " " + str2);
        }

        public static void Question(string str1, string str2)
        {
            Write(questionColor, str1);
            Write(ConsoleColor.DarkYellow, "? ");
            WriteLine(messageColor, str2);
        }

        public static void Status(string status)
        {
            Write(statusColor, status);
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

        public static void PrintVersion(string programName, string programVersion, string programYear)
        {
            Write(ConsoleColor.Green, programName + " ");
            Write(ConsoleColor.White, programVersion);
            NewLine();
            Write(ConsoleColor.White, "Copyright © " + programYear + " ");
            Write(ConsoleColor.DarkYellow, "Third ");
            Write(ConsoleColor.Yellow, "3");
            Write(ConsoleColor.DarkYellow, "ye Software");
            NewLine();
        }

        public static System.IO.TextWriter PrintUsage(string programName, string programVersion, string programYear)
        {
            string programDescription = "Replicate a Stored Procedure to remote servers.";

            PrintVersion(programName, programVersion, programYear);

            WriteLine(ConsoleColor.Cyan, programDescription);
            Write(ConsoleColor.White, "USAGE: ");
            NewLine();

            /*
            replicatesp - n SP_FILE_DEFINITION[-i][-f][-v]
            */
            UsageProgramName(programName);
            UsageOption("n", "SP_FILE_DEFINITION");
            UsageFlag("i", optional: true);
            UsageFlag("f", optional: true);
            UsageFlag("q", optional: true);
            UsageFlag("t", optional: true);
            UsageFlag("d", optional: true);
            NewLine();
            Write(ConsoleColor.White, "Options:");
            NewLine();


            PrintOption(LongName: "name",
                        HelpText: "Name of the file name with the text definition of the Stored Procedure to create.",
                        Flag: false, Example: "SP_NAME");
            PrintOption(LongName: "quiet",
                        HelpText: "Do not print any information to standard output.",
                        Flag: true);
            PrintOption(LongName: "interactive",
                        HelpText: "Run in interactive mode.",
                        Flag: true);
            PrintOption(LongName: "force",
                        HelpText: "Force creating the Stored Procedure even if it exists at destination.",
                        Flag: true);
            PrintOption(LongName: "test",
                        HelpText: "Run in test mode showing what is going to be done but making no changes.",
                        Flag: true);
            PrintOption(LongName: "debug",
                        HelpText: "Run in debug mode showing all errors in detail.",
                        Flag: true);
            PrintOption(LongName: "config",
                        HelpText: "Configuration file with the destination servers and databases where to create the Stored Procedures. (defaults to replicatsp.yaml)",
                        Flag: false, Example: "replicatesp.yaml");
            PrintOption(LongName: "version",
                        HelpText: "Print this help message and exit.",
                        Flag: true);
            PrintOption(LongName: "help",
                        HelpText: "Print the version number and exit.",
                        Flag: true);
            return null;
        }

        static public void UsageProgramName(string str)
        {
            Write(programColor, "    " + str + " ");
        }

        static public void UsageOption(string opt, string val, bool optional = false)
        {
            if (optional) Write(separatorColor, "[");
            Write(optionColor, " -" + opt + " ");
            Write(valueColor, val);
            if (optional) Write(separatorColor, "]");
        }

        static public void UsageFlag(string opt, bool optional = false)
        {
            if (optional) Write(separatorColor, " [");
            Write(optionColor, "-" + opt);
            if (optional) Write(separatorColor, "]");
        }

        static public void UsageValue(string str)
        {
            Write(valueColor, " " + str + " ");
        }

        static public void PrintOption(string LongName, string HelpText, bool Flag, string Example = "")
        {
            const int nameLength = 13;
            const int valueLenth = 18;
            const int helpLenght = 71;
            char ShortName = LongName[0];
            string value = "";

            Write(optionColor, "    -" + ShortName);
            Write(separatorColor, ", ");
            Write(optionColor, "--" + LongName.PadRight(nameLength));

            if (!Flag)
                value = Example;
            Write(valueColor, value.PadRight(valueLenth));
            PrintHelpText(HelpText, helpColor, nameLength, valueLenth, helpLenght);
        }

        static public void StatusError(bool error)
        {
            Write(separatorColor, " [");
            if (error) {
                Write(errorColor, "error");
            }
            else {
                Write(helpColor, "OK");
            }
            
            Write(separatorColor, "]");
        }

        static public void PrintHelpText(string HelpText, ConsoleColor color, int nameLength, int valueLenth, int helpLenght)
        {
            string line = null;
            char[] wordDelimiter = new[] { ' ', '\t', ',', '.', '!', '?', ';', ':', '/', '\\', '[', ']', '<', '>', '@', '"', '\'' };
            string[] words = HelpText.Split(wordDelimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                if ((line + word).Length > helpLenght)
                {
                    WriteLine(color, line);
                    line = null;
                    Write(color, "".PadLeft(nameLength + valueLenth + 10));
                }
                line += word + " ";
            }
            line = line.Remove(line.Length - 1, 1) + ".";
            WriteLine(color, line);

        }

        static public void PrintConfiguration(Configuration config)
        {
            foreach (var item in config.Connections)
            {
                Write(ConsoleColor.DarkGreen, "Name(" + item.Name + ")");
                Write(ConsoleColor.Yellow, ", Server(" + item.Server + ")");
                Write(ConsoleColor.Yellow, ", Database(" + item.Database + ")");
                Write(ConsoleColor.Yellow, ", User(" + item.User + ")");
                WriteLine(ConsoleColor.Yellow, ", Password(" + item.Password + ")");
            }
        }

    }
}
