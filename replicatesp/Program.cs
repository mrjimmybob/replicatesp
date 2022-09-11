using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization.NamingConventions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static replicatesp.Interactive;
using System.Xml.Linq;
using static replicatesp.Spinner;
using System.ComponentModel.Design;

namespace replicatesp
{


    internal class Program
    {
        private static readonly int versionMajor = 1;
        private static readonly int versionMinor = 0;
        private static readonly int versionRevision = 0;
        private static readonly string programYear = "2022";
        private static string strProgramVersion = null;
        private static string strProgramName = null;

        static private List<Task<bool>> SqlProcesses = new List<Task<bool>>();

        static void Main(string[] args)
        {
            SetProgramVersion();
            SetProgramName();
            // -n ..\..\..\SP_DEF.sql -c ..\..\..\replicatesp.yaml -f -i -q -t
            string[] putosArgumentos = { "-n", "..\\..\\..\\SP_DEF.sql", "-c", "..\\..\\..\\replicatesp.yaml", "-d", "-i", "-f"};
            /*
                Print.Write(ConsoleColor.Magenta, "Connecting [");
                new Spinner(Symbols.SpinnerType.Braily, ConsoleColor.Magenta, 0, -1);
                Print.Write(ConsoleColor.Magenta, " ]");
                Print.NewLine();
                Console.ReadKey();
                Print.Write(ConsoleColor.Cyan, "Connecting [");
                new Spinner(Symbols.SpinnerType.Bars, ConsoleColor.Cyan);
                Print.Write(ConsoleColor.Cyan, " ]");
            */
            var parser = new CommandLine.Parser(with => {
                with.AutoVersion = false;
                with.EnableDashDash = true;
                with.AutoHelp = false;
                with.HelpWriter = null; // CmdLineOptions.Usage()
            });
            var result = parser.ParseArguments<CmdLineOptions>(putosArgumentos);
            //var result = parser.ParseArguments<CmdLineOptions>(args);
            result.WithParsed(options => RunOptions(options)) // options is an instance of CmdLineOptions type
                  .WithNotParsed(errors => CmdLineOptions.ThrowOnParseError(result, errors, strProgramName, strProgramVersion, programYear)); // errors is a sequence of type IEnumerable<Error>
        }

        public static void SetProgramVersion()
        {
            strProgramVersion = versionMajor.ToString() + "." + versionMinor.ToString() + "." + versionRevision.ToString();
        }

        public static string GetProgramVersion()
        {
            return strProgramVersion;
        }

        public static void SetProgramName()
        {
            strProgramName = AppDomain.CurrentDomain.FriendlyName;
        }

        public static string GetProgramName()
        {
            return strProgramName;
        }

        public static void RunOptions(CmdLineOptions opts)
        {
            string spFileDescription = String.Empty;
            List<string> storedProcedures = null;

            // handle special cases (help, version)
            if (opts.Help)
                PrintUsage();
            if (opts.Version)
                PrintVersion();
            // Hanlde flags
            string forcedOffTag = String.Empty;
            if (opts.Interactive && opts.Quiet
                || opts.Debug && opts.Quiet) {
                // Quiet and Interactive or Debug modes are incompatible with quiet mode
                // (interactive or debug take prececdence over quiet)
                forcedOffTag = " (interactive or debug mode take precedence over quiet mode)";
                opts.Quiet = false;
            }
            if (!opts.Quiet) Print.Info("Quiet mode:       ", opts.Quiet ? "On" : "Off" + forcedOffTag);
            if (!opts.Quiet) Print.Info("Force mode:       ", opts.Force ? "On" : "Off");
            if (!opts.Quiet) Print.Info("Interactive mode: ", opts.Interactive ? "On" : "Off");
            if (!opts.Quiet) Print.Info("Test mode:        ", opts.Test ? "On" : "Off");
            if (!opts.Quiet) Print.Info("Debug mode:       ", opts.Debug ? "On" : "Off");

            if (opts.Test) Print.Error("Test mode is not implemented yet!", "Error", "Lazy programmer error");

            if (IsFileNameOK(opts.Name))
            { 
                if (!opts.Quiet) Print.Info("Going to replicate contents of file: ", opts.Name);
                spFileDescription = ReadFileContents(opts.Name);
                if (opts.Debug) Print.Error(opts.Name, "Debug", Environment.NewLine + spFileDescription);
                if (null == spFileDescription || spFileDescription.Length < 15) {
                    Print.Error("Nothing to do", "Error", opts.Name + " is NULL or malformed");
                    Environment.Exit(-1);
                }
                if (spFileDescription != null && ContainsForbidenContents(spFileDescription)) {
                    Print.Error("I refuse to use DROP in a Database", "Security check", "Check file: " + opts.Name);
                    Environment.Exit(-1);
                }
                // Print.WriteLine(ConsoleColor.Magenta, opts.Name + ":");
                // Print.WriteLine(ConsoleColor.White, spFileDescription);
                //string str = CreateSpBackupName(spFileDescription);
                storedProcedures = ExtractStoredProcedures(spFileDescription);
            }
            else {
                Print.Error("Error reading file", opts.Name, "A fatal error has occurred, I refuse to continue.");
                Environment.Exit(0);
            }
            // hanlde config
            if (opts.ConfigFile != null && opts.ConfigFile.Length > 0)
            {
                if (!opts.Quiet) Print.Info("Reading configuration from: ", opts.ConfigFile);

                Configuration configuration = ReadConfiguration(opts.ConfigFile);
                if (configuration != null)
                {
                    if (opts.Debug) Print.Error(opts.ConfigFile, "Debug", Environment.NewLine + ReadFileContents(opts.ConfigFile));
                    answer response = opts.Interactive ? QuestionYesCancel("Command line options and configuration loaded. Continue") : answer.yes;
                    switch (response) {
                        case answer.cancel:
                        case answer.no:
                            CancelExecution();
                            break;
                        case answer.yes:
                            foreach (var con in configuration.Connections)
                            {
                                ProcessConfigItem(storedProcedures, con, opts);
                            }
                            break;
                    }
                    
                    // CollectTaskInfo(sqlProcesses);
                    if (!opts.Quiet) Print.Info("Finished!", " The program has ended normally.");
                }
                else {
                    Print.Error("Error reading configuration file", opts.ConfigFile, "No configuration has been loaeded, I cannot continue.");
                    Environment.Exit(0);
                }
            }
            
        }


        static public bool IsFileNameOK(string fileName)
        {
            if (fileName == null)
            {
                // -n | --name is required
                Print.PrintUsage(strProgramName, strProgramVersion, programYear);
                Print.Error("Option -n (or --name) is required.", "Argument error", "A required argument was not found. I refuse to continue.");
                Environment.Exit(0);
            }
            if (fileName != null && fileName.Length > 0)
            {
                if (!File.Exists(fileName))
                {
                    // -n FILE, is given but FILE does not exist
                    Print.PrintUsage(strProgramName, strProgramVersion, programYear);
                    Print.Error("The file '" + fileName + "' cannot be found", "File not found", "The file must exist and you must have read access to it.");
                    Environment.Exit(0);
                }
            }
            return true;
        }

        public static void PrintUsage()
        {
            Print.PrintUsage(strProgramName, strProgramVersion, programYear);
            Environment.Exit(0);
        }

        public static void PrintVersion()
        {
            Print.PrintVersion(strProgramName, strProgramVersion, programYear);
            Environment.Exit(0);
        }

        static string ReadFileContents(string fileName)
        {
            try
            {
                // Open the text file using a stream reader.
                using var sr = new StreamReader(fileName);
                // Read the stream as a string, and write the string to the console.
                string configFile = sr.ReadToEnd();
                // Console.WriteLine(configFile);
                return configFile;
            }
            catch (IOException e)
            {
                Print.Error("The file '" + fileName + "'could not be read", "Error", e.Message);
                return null;
            }
        }

        static Configuration ReadConfiguration(string configFileName)
        {
            string configFile = ReadFileContents(configFileName);
            if (configFile != null)
            {
                try
                {
                    var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    var Configuration = deserializer.Deserialize<Configuration>(configFile);

                    // Print.PrintConfiguration(Configuration);
                    return Configuration;
                }
                catch (YamlDotNet.Core.SemanticErrorException e)
                {
                    Print.Error("Error reading configuration file", "Error", e.Message);
                }
                catch (YamlDotNet.Core.YamlException e)
                {
                    Print.Error("Error reading configuration file", "Error", e.Message);
                }
            }
            return null;
        }

        static void ProcessConfigItem(List<string> storedProcedureList, Data con, CmdLineOptions opts)
        {
            DB db = new DB(con.Server, con.User, con.Password, con.Database);
            if (!db.Error())
            {
                foreach (string sp in storedProcedureList) {
                    if (opts.Interactive) 
                    {
                        string spName = GetStoredProcedureName(sp);
                        answer response = answer.yes;
                        if (opts.Debug) Print.Error("SP:", "Debug", Environment.NewLine + sp);
                        if (opts.Interactive)
                        {
                            Print.Info("Info:", "I am going to replicate (" + GetStoredProcedureOperation(sp) 
                                + ") " + spName + " to " + con.Name + " (" + con.Database + ")");
                            response = QuestionYesSkipCancel("Do you want to continue");
                        }
                        switch (response)
                        {
                            case answer.cancel:
                                CancelExecution();
                                break;
                            case answer.skip:
                                if (!opts.Quiet) Print.Info("Skipped:", "replicating " + spName + " to " + con.Name + " (" + con.Database + ")");
                                continue;
                        }
                    }

                    Spinner spinner = null;
                    if (!opts.Quiet)
                    {
                        PrintItem(con, sp);
                        if (!opts.Interactive) spinner = new Spinner(Symbols.SpinnerType.SlidingO, ConsoleColor.Yellow);
                    }
                    RunTask(db, sp);
                    if (RetryOnErrorOK(opts, db, sp))
                    {
                        // You retried and it worked, reset error...
                        db.ResetError();
                    }
                    if (!opts.Quiet) {
                        if (!opts.Interactive) spinner.Delete(db.Error());
                        Print.StatusError(db.Error());
                        Print.NewLine();
                    }
                    // RunParallellTask(db, sp);
                    if (opts.Debug && db.Error()) Print.Error(db.getErrorMessage(), "Debug", db.getErrorDetail());
                    db.ResetError();
                }
            }
            else {
                if (opts.Debug) Print.Error(db.getErrorMessage(), "Debug" + con.Name, db.getErrorDetail());
            }
        }

        public static bool RetryOnErrorOK(CmdLineOptions opts, DB db, string sp)
        {
            if (opts.Force)
            {
                // Only retry on forced execution
                switch (db.getErrorNumber())
                {
                    case 2714: // Name exists (when creating)
                        // Rename procedure and retry CREATE
                        if (RenameStoredProcedure(opts, db, sp))
                        {
                            RunTask(db, sp);
                            if (opts.Debug && db.Error()) Print.Error(db.getErrorMessage(), "Debug", db.getErrorDetail());
                            if (db.Error()) return false;
                            db.ResetError();
                            return true;
                        }
                        return false;
                    case 208: // Name does not exist (when altering)
                        // rename ALTER to CREATE and retry
                        string newSp = getRespecifiedSPOperation(opts, sp);
                        if (opts.Debug) Print.Error("Changed ALTER to CREATE", "Debug", Environment.NewLine + newSp);
                        if (newSp == string.Empty) return false;
                        RunTask(db, newSp);
                        if (opts.Debug && db.Error()) Print.Error(db.getErrorMessage(), "Debug", db.getErrorDetail());
                        if (db.Error()) return false;
                        db.ResetError();
                        return true;
                }
            }
            return true;
        }

        public static string getRespecifiedSPOperation(CmdLineOptions opts, string sp)
        {
            if (sp == null || sp.Length == 0) CancelExecution("Aborting", "No stored procedure to recalify");
            // We have to change ALTER to CREATE
            if (opts.Debug) Print.Info(Environment.NewLine + "Destination object does not exist", "I am going to try and CREATE instead of ALTER");
            if (-1 == sp.ToUpper().IndexOf("ALTER")) return string.Empty;
            string newSp = ReplaceFirst(sp, "Alter", "CREATE");
            newSp = ReplaceFirst(newSp, "ALTER", "CREATE");
            return ReplaceFirst(newSp, "alter", "CREATE");
        }

        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static bool RenameStoredProcedure(CmdLineOptions opts, DB db, string sp)
        {
            if (sp == null || sp.Length == 0) CancelExecution("Aborting", "No stored procedure to rename");
            string oldName = GetStoredProcedureName(sp);
            string newName = CreateSpBackupName(sp);
            if (opts.Debug) Print.Info(Environment.NewLine + "Destination object exists", "I am going to try and rename " + oldName + " to " + newName + " and try again");
            db.ResetError();
            db.RenameStoredProcedure(oldName, newName);
            if (db.Error())
            {
                if (opts.Debug) Print.Error("Error renaming " + oldName + " to " + newName, "SQL Error",
                              db.getErrorMessage() + " (" + db.getErrorDetail() + ")");
                return false;
            }
            return true;
        }

        public static string CreateSpBackupName(string sourcestring)
        {
            if (sourcestring == null || sourcestring.Length == 0) CancelExecution("Aborting", "No stored procedure to create a backup name from");
            Regex re = new Regex(@"PROCEDURE (?:dbo\.|\[dbo\]\.)*\[*([a-zA-Z0-9_ -]+)\]*",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = re.Matches(sourcestring);

            foreach (Match match in matches.Cast<Match>())
            {
                GroupCollection groups = match.Groups;
                return groups[1].Value + "_" + DateTime.Now.ToString("yyyyMMddHHmm");
            }
            CancelExecution("Aborting", "Error creating backup name for stored procedure");
            return string.Empty;
        }

        public static string GetStoredProcedureName(string sourcestring)
        {
            if (sourcestring == null || sourcestring.Length == 0) CancelExecution("Aborting", "No stored procedure to get name from");
            Regex re = new Regex(@"PROCEDURE (?:dbo\.|\[dbo\]\.)*\[*([a-zA-Z0-9_ -]+)\]*",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = re.Matches(sourcestring);
            foreach (Match match in matches.Cast<Match>())
            {
                GroupCollection groups = match.Groups;
                return groups[1].Value;
            }
            CancelExecution("Aborting", "Error getting name from stored procedure");
            return string.Empty;
        }

        public static string GetStoredProcedureOperation(string sourcestring)
        {
            if (sourcestring == null || sourcestring.Length == 0) CancelExecution("Aborting", "No stored procedure found");
            Regex re = new Regex(@"\r*\n*\s*\b(ALTER|CREATE)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = re.Matches(sourcestring);
            foreach (Match match in matches.Cast<Match>())
            {
                GroupCollection groups = match.Groups;
                return groups[1].Value;
            }
            CancelExecution("Aborting", "Error getting operation from stored procedure");
            return string.Empty;
        }

        public static List<string> ExtractStoredProcedures(string sourcestring)
        {
            if (sourcestring == null || sourcestring.Length == 0) CancelExecution("Aborting", "No stored procedure to extract");
            List<string> result = new List<string>();
            Regex re = new Regex(@"\b(ALTER|CREATE)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = re.Matches(sourcestring);
            int start = 0;
            foreach (Match match in matches.Cast<Match>())
            {
                GroupCollection groups = match.Groups;
                int newMatch = groups[0].Index;
                if (start != newMatch)
                {
                    int end = newMatch;
                    result.Add(sourcestring[start..end]);
                    // Print.Info("SP: ", sourcestring[start..end]);
                    start = end;
                }
            }
            if (sourcestring.Length > start) {
                result.Add(sourcestring[start..]);
                // Print.Info("SP: ", sourcestring.Substring(start));
            }
            return result;
        }

        public static bool ContainsForbidenContents(string sp)
        {
            if (sp.ToUpper().Contains("DROP")) {
                return true;
            }
            return false;
        }

        public static void PrintItem(Data item, string sp)
        {
            Print.Write(ConsoleColor.DarkGreen, "Replicating: ");
            Print.Write(ConsoleColor.DarkYellow, item.Name + ": ");
            Print.Write(ConsoleColor.Cyan, item.Server);
            Print.Write(ConsoleColor.Yellow, ", Database: ");
            Print.Write(ConsoleColor.Cyan, item.Database);
            Print.Write(ConsoleColor.Yellow, ", Stored Procedure: ");
            Print.Write(ConsoleColor.Cyan, GetStoredProcedureName(sp) + "...");
        }
                
        public static void CancelExecution(string title = null, string message = null)
        {
            if (title == null && message == null)
                Print.Info("Aborting.", "Ending program execution at users request.");
            else
                Print.Info(title, message);
            Environment.Exit(0);
        }


        public static bool RunTask(DB db, string sp)
        {
            db.RunQuery(sp);
            if (db.Error())
            {
                // Print.Error(db.getErrorMessage(), "Error", db.getErrorDetail());
                return false;
            }
            return true;
        }

        public static void RunParallellTask(DB db, string sp)
        {
            SqlProcesses.Add(
            Task.Run(() => {
                return RunTask(db, sp);
            }));
        }

        public static void CollectTaskInfo(List<Task<bool>> sqlProcesses)
        {
            try
            {
                Task.WaitAll(SqlProcesses.ToArray());
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.Flatten().InnerExceptions)
                    Print.Error("Process error", ex.Message);
            }
            foreach (var sqlProcess in SqlProcesses) {
                Print.Info("Status of completed tasks: ", "Task #" + sqlProcess.Id + ": " + sqlProcess.Status + ", OK: " + sqlProcess.Result.ToString());
            }
        }

    }
}
