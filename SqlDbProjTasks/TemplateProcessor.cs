using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace SqlDbProjTasks {
    internal class TemplateProcessor {
        private static readonly Regex PlaceHolderRegex = new Regex(@"<<<(?<term>\w+)>>>", RegexOptions.Compiled);
        private readonly FileInfo file;
        public TemplateProcessor(FileInfo file) {
            this.file = file;
        }

        public string Process(string template) {
            if (string.IsNullOrEmpty(template)) {
                return string.Empty;
            }

            return PlaceHolderRegex.Replace(template, TermEvaluator);
        }

        private string TermEvaluator(Match match) {
            var term = match.Groups["term"].Value.ToLower();
            switch(term) {
                case "index":
                    return file.Index.ToString();
                case "path":
                    return file.FilePath;
                case "name":
                    return Path.GetFileName(file.FilePath);
            }

            throw new InvalidOperationException(string.Format("Template term '{0}' is not supported.", term));
        }
    }
}
