using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Horn.Core.extensions;
using log4net;

namespace Horn.Core.Utils.CmdLine
{
    public class SwitchParser
    {
        #region console help text

        public const string HelpText =
@"THE HORN PACKAGE MANAGERS
                                          
http://code.google.com/p/hornget/

Usage : horn -install:<component>
Options :
    -rebuildonly                Do not check for the latest source code.
    -version:<version_number>   The specific version of a package.";

        #endregion        

        private readonly TextWriter output;
        private readonly Parameter[] paramTable;
        private readonly Dictionary<string, IList<string>> parsedArgs;
        private static readonly ILog log = LogManager.GetLogger(typeof(SwitchParser));

        public ICommandArgs CommandArguments
        {
            get
            {
                return new CommandArgs(parsedArgs);
            }
        }

        public Dictionary<string, IList<string>> ParsedArgs
        {
            get { return parsedArgs; }
        }

        public virtual bool IsAValidRequest()
        {
            if (IsHelpTextSwitch())
                return false;

            return IsValid();            
        }

        public virtual bool IsHelpTextSwitch()
        {
            return ParsedArgs != null && ParsedArgs is HelpReturnValue;
        }

        public virtual bool IsValid()
        {
            var ret = true;

            foreach (var paramRow in paramTable)
            {
                var arg = ParsedArgs.ContainsKey(paramRow.Key) ? ParsedArgs[paramRow.Key] : null;

                if (arg == null)
                {
                    if (paramRow.Required)
                        ret = OutputValidationMessage(string.Format("Missing required argument key: {0}.", paramRow.Key));

                    continue;
                }

                if ((paramRow.RequiresArgument) && (arg.Count == 0 || string.IsNullOrEmpty(arg[0])))
                    return OutputValidationMessage(string.Format("Missing argument value for key: {0}.", paramRow.Key));

                if (arg.Count > 1 && !paramRow.Reoccurs)
                    ret = OutputValidationMessage(string.Format("Argument key cannot reoccur: {0}.", paramRow.Key));
            }

            foreach (var keyValuePair in ParsedArgs)
            {
                var paramRow = Array.Find(paramTable, match => match.Key == keyValuePair.Key);

                if (paramRow == null)
                    ret= OutputValidationMessage(string.Format("Argument key unknown: {0}.", keyValuePair.Key));
            }

            return ret;
        }

        public virtual void OutputHelpText()
        {
            output.WriteLine(HelpText);
        }

        public virtual bool OutputValidationMessage(string message)
        {
            output.WriteLine(message);

            string usage = "USAGE:" + Environment.NewLine;

            foreach (var paramRow in paramTable)
                usage += string.Format("{0}-{1}{2}{3}{4}" + Environment.NewLine, paramRow.Required ? string.Empty : "[", paramRow.Key, paramRow.RequiresArgument ? ":{}" : "", paramRow.Reoccurs ? "*" : "", paramRow.Required ? "" : "]");

            output.WriteLine();
            output.WriteLine(usage);

            return false;
        }

        private Dictionary<string, IList<string>> Parse(string[] args)
        {
            const string argsRegex = @"-([a-zA-Z_][a-zA-Z_0-9]{0,}):?((?<=:).{0,})?";
            string name;
            Match match;

            var parsedArgs = new Dictionary<string, IList<string>>();

            if ((!args.HasElements()) || (args[0].ToLower().Equals("-help")))
            {
                OutputHelpText();

                return new HelpReturnValue();
            }

            foreach (string arg in args)
            {
                match = Regex.Match(arg, argsRegex, RegexOptions.IgnorePatternWhitespace);

                if ((match == null) || (!match.Success) || (match.Groups.Count != 3))
                    continue;

                name = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                name = name.ToLower();

                if (!parsedArgs.ContainsKey(name))
                    parsedArgs.Add(name, new List<string>());

                if (!parsedArgs[name].Contains(value))
                    parsedArgs[name].Add(value);
            }

            LogArguments(parsedArgs);

            return parsedArgs;
        }

        private static void LogArguments(Dictionary<string, IList<string>> args)
        {
            foreach (var arg in args)
            {
                log.InfoFormat("Command {0} was issued with values:", arg.Key);

                foreach (var value in arg.Value)
                    log.InfoFormat("{0}\n", value);
            }
        }

        public SwitchParser(TextWriter output, string[] args)
        {
            this.output = output;

            var parameters = new List<Parameter>
                                 {
                                     new Parameter("install", true, true, false),
                                     new Parameter("rebuildonly", false, false, false),
                                     new Parameter("version", false, true, false),
                                     new Parameter("refresh", false, false, false)
                                 };

            paramTable = parameters.ToArray();

            parsedArgs = Parse(args);
        }
    }
}