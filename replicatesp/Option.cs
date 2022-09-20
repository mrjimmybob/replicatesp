using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace replicatesp
{
    internal class Option
    {
        public readonly string LongName;
        public readonly char   ShortName;
        public readonly string HelpText;
        public readonly bool   Required  = false;
        public readonly bool   Flag      = false;
        public readonly string Default   = null;
        private string          Value     = null;
        public readonly string Example   = null;

        public Option(string longname, string helptext, char shortname = '\x0', bool required = false, bool flag = false, string def = null, string example = null)
        {
            ShortName = shortname;
            LongName  = longname;
            HelpText  = helptext;
            Required  = required;
            Flag      = flag;
            Default   = def;
            Example   = example;

            if ('\x0' == ShortName)
            {
                ShortName = LongName[0];
            }
        }

        public string GetValue()
        {
            return Value;
        }

        public void SetValue(string value)
        {
            Value = value;
        }
        
        public void PrintHelpText(ConsoleColor Color, int width) 
        {
            string line = null;
            char[] wordDelimiter = new[] { ' ', '\t', ',', '.', '!', '?', ';', ':', '/', '\\', '[', ']', '(', ')', '<', '>', '@', '"', '\'' };
            string[] words = HelpText.Split(wordDelimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words) {
                if ((line + word).Length > width) {
                    Print.WriteLine(Color, line);
                    line = null;
                    Print.Write(Color, "                                              ");
                }
                line += word + " ";
            }
            line = line.Remove(line.Length - 1, 1) + ".";
            Print.WriteLine(ConsoleColor.DarkGreen, line);

        }

        public void PrintOption()
        {
            Print.Write(ConsoleColor.White, "          -" + ShortName);
            Print.Write(ConsoleColor.DarkGray, ", ");
            Print.Write(ConsoleColor.White, "--" + String.Format("{0,-15}", LongName));

            string value = "";
            if (!Flag)
                value = Example;
            Print.Write(ConsoleColor.DarkGray, String.Format("{0,-15}", value));
            PrintHelpText(ConsoleColor.DarkGreen, 68);
        }
    }
 }
