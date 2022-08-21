using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace replicatesp
{
    internal class Arguments
    {

        public static Option[] options = new Option[]
        {
            new Option(longname: "name",
                       helptext: "Name of, or file name of text definition of, the Stored Procedure to create.",
                       example:  "SP_NAME",
                       required: true),
            new Option(longname: "config",
                       example:  "config.json",
                       helptext: "Configuration file with the destination servers and databases where to create the Stored Procedures."),
            new Option(longname: "server",
                       example:  "SERVER",
                       helptext: "Name of source Server to get the Stored Procedure from."),
            new Option(longname: "database",
                       example:  "DATABASE",
                       helptext: "Name of source Database to get the Stored Procedure from."),
            new Option(longname: "force",
                       helptext: "Force creating the Stored Procedure even if it exists at destination."),
            new Option(longname: "interactive",
                       helptext: "Run in interactive mode."),
            new Option(longname: "quiet",
                       helptext: "Do not print any information to standard output."),
        };

        public void ParseArgs(Option[] options, string[] args)
        { 
        }

        public static void Usage()
        {
            string programName = "replicatesp";
            string programVersion = "1.0.0";
            string programDescription = "Replicate a Stored Procedure to remote servers.";
            string programYear = "2022";

            Print.Write(ConsoleColor.Green, programName + " ");
            Print.Write(ConsoleColor.White, programVersion);
            Print.NewLine(); 
            Print.Write(ConsoleColor.White, "Copyright © " + programYear + " ");
            Print.Write(ConsoleColor.DarkYellow, "Third ");
            Print.Write(ConsoleColor.Yellow, "3");
            Print.Write(ConsoleColor.DarkYellow, "ye Software");
            Print.NewLine();
            Print.WriteLine(ConsoleColor.Cyan, programDescription);
            Print.Write(ConsoleColor.White, "USAGE: ");
            Print.NewLine();
            Print.Write(ConsoleColor.Green, "    " + programName + " ");
            Print.Write(ConsoleColor.White, "-n ");
            Print.Write(ConsoleColor.DarkGray, "SP_FILE_DEFINITION [");
            Print.Write(ConsoleColor.White, "-i");
            Print.Write(ConsoleColor.DarkGray, "] [");
            Print.Write(ConsoleColor.White, "-f");
            Print.Write(ConsoleColor.DarkGray, "] [");
            Print.Write(ConsoleColor.White, "-v");
            Print.Write(ConsoleColor.DarkGray, "]");
            Print.NewLine();
            Print.Write(ConsoleColor.White, "Options:");
            Print.NewLine();
            foreach (var option in options) {
                option.PrintOption();
            }
        }
    }
}
