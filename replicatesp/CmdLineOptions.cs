using CommandLine;
using CommandLine.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace replicatesp
{
    //public class Options
    //{
    //    public char   ShortName;
    //    // public string LongName;
    //    public bool   Required;
    //    public bool   Default;
    //    public string HelpText;
    //}

    public class CmdLineOptions
    {
        //static Dictionary<string, Options> QuietOption = new Dictionary<string, Options>() {
        //    { "quiet", new Options { ShortName='q', Required=false, Default = false, HelpText = "Do not print any information to standard output." } },

        //};
        //char s =   QuietOption["quiet"].ShortName;

        [Option('q', "quiet", Required = false, Default = false, HelpText = "Do not print any information to standard output.")]
        public bool Quiet { get; set; }

        [Option('i', "interactive", Required = false, Default = false, HelpText = "Run in interactive mode.")]
        public bool Interactive { get; set; }

        [Option('f', "force", Required = false, Default = false, HelpText = "Force creating the Stored Procedure even if it exists at destination.")]
        public bool Force { get; set; }

        [Option('t', "test", Required = false, Default = false, HelpText = "Run in test mode showing what is going to be done but making no changes.")]
        public bool Test { get; set; }

        [Option('d', "debug", Required = false, Default = false, HelpText = "Run in debug mode showing all errors in detail.")]
        public bool Debug { get; set; }

        [Option('c', "config", Required = false, Default = "replicatesp.yaml", HelpText = "Configuration file with the destination servers and databases where to create the Stored Procedures. (defaults to replicatsp.yaml)")]
        public string ConfigFile { get; set; }

        [Option('n', "name", Required = false, HelpText = "Name of the file name with the text definition of the Stored Procedure to create.")]
        public string Name { get; set; }

        [Option('h', "help", Required = false, Default = false, HelpText = "Print this help message.")]
        public bool Help { get; set; }

        [Option('v', "version", Required = false, Default = false, HelpText = "Print the version number and exit.")]
        public bool Version { get; set; }

        public static void ThrowOnParseError<T>(ParserResult<T> result, IEnumerable<Error> errors,
                                string strProgramName, string strProgramVersion, string programYear)
        {
            Print.PrintUsage(strProgramName, strProgramVersion, programYear);

            var builder = SentenceBuilder.Create();
            var errorMessages = HelpText.RenderParsingErrorsTextAsLines(result,
                builder.FormatError, builder.FormatMutuallyExclusiveSetErrors, 1);
            var excList = errorMessages.Select(msg => new ArgumentException(msg)).ToList();

            if (excList.Any())
            {
                foreach (var exc in excList)
                {
                    Print.Error(exc.Message.ToString().Trim(), "Argument error");
                }
            }
            Print.Write(ConsoleColor.Red, "Execution terminated with errors.");
            Environment.Exit(0);
        }

    }

}
