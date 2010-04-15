using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;


namespace SqlDbProjTasks {
    public class Combine : Task {

        [Required]
        public ITaskItem[] Source {
            get;
            set;
        }

        [Required]
        public string Output { get; set; }

        public string Before { get; set; }
        public string After { get; set; }
        public bool AllowEmptyIndex { get; set; }

        public Combine() {
            AllowEmptyIndex = true;
        }

        public override bool Execute() {
            if (Source.Length == 0) {
                return true;
            }

            var sortedFiles = SortFileList(Source);
            var beforeTemplate = string.IsNullOrEmpty(Before) ? string.Empty : File.ReadAllText(Before);
            var afterTemplate = string.IsNullOrEmpty(After) ? string.Empty : File.ReadAllText(After);
            using (var output = File.CreateText(Output)) {
                foreach(var file in sortedFiles) {
                    PutTemplate(output, beforeTemplate, file);
                    output.WriteLine(File.ReadAllText(file.FilePath));
                    PutTemplate(output, afterTemplate, file);
                }
                output.Flush();
            }

            return true;
        }

        private static void PutTemplate(TextWriter writer, string template, FileInfo fileInfo) {
            var processor = new TemplateProcessor(fileInfo);
            var result = processor.Process(template);
            if(!string.IsNullOrEmpty(result)) {
                writer.WriteLine(result);
            }
        }

        private IList<FileInfo> SortFileList(ITaskItem[] sourceFiles) {
            return sourceFiles
                .Select(item => new FileInfo(item.ItemSpec, AllowEmptyIndex))
                .OrderBy(file => file.Index)
                .ToList();
        }
    }
}
