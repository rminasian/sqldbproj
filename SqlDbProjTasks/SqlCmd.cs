using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Text;

namespace SqlDbProjTasks {
    public class SqlCmd : Task {

        [Required]
        public string Server { get; set; }
        [Required]
        public string Database { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }

        public string ScriptFileName { get; set; }

        /// <summary>
        /// Script variables
        /// </summary>
        /// <example>var1=value1 var2=value2</example>
        public string Variables {
            get;
            set;
        }

        /// <summary>
        /// Query to execute
        /// </summary>
        /// <remarks>
        /// <para>Do not enclose query with quotes</para>
        /// <para>ScriptFileName has higher priority then Query</para>
        /// </remarks>
        public string Query { get; set; }

        public bool IgnoreExitCode { get; set; }

        [Output]
        public int ExitCode { get; private set; }


        public SqlCmd() {
            IgnoreExitCode = false;
            ExitCode = 0;
        }

        public override bool Execute() {
            if (string.IsNullOrEmpty(ScriptFileName) && string.IsNullOrEmpty(Query)) {
                Log.LogError("ScriptFileName or Query must be set.");
                return false;
            }
            try {
                var proc = new Process();
                proc.StartInfo.FileName = "sqlcmd.exe";
                proc.StartInfo.Arguments = CreateCommandLine();
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                string log = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                Log.LogMessageFromText(log, MessageImportance.Normal);
                ExitCode = proc.ExitCode;
                if (ExitCode != 0 && !IgnoreExitCode) {
                    Log.LogError("Error executing script. See build log.");
                    return false;
                }

                return true;
            } catch (Exception ex) {
                Log.LogErrorFromException(ex);
                ExitCode = -1;
                return false;
            }
        }

        //sqlcmd -S <Server> -d <Database> -U <User> -P <Password> -b -i <ScriptFileName> [-v <Variables>]
        private string CreateCommandLine() {
            var commandLine = new StringBuilder();
            commandLine.Append("-S ");
            commandLine.Append(Server);
            commandLine.Append(" -d ");
            commandLine.Append(Database);
            commandLine.Append(" -U ");
            commandLine.Append(User);
            commandLine.Append(" -P ");
            commandLine.Append(Password);
            commandLine.Append(" -b");
            if (!string.IsNullOrEmpty(ScriptFileName)) {
                commandLine.Append(" -i ");
                commandLine.Append(ScriptFileName);
            } else if (!string.IsNullOrEmpty(Query)) {
                commandLine.AppendFormat(" -Q \"{0}\"", Query);
            }

            if (!string.IsNullOrEmpty(Variables)) {
                commandLine.Append(" -v ");
                commandLine.Append(Variables);
            }

            return commandLine.ToString();
        }
    }
}