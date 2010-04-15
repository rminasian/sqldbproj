using System;
using System.IO;
using System.Text.RegularExpressions;


namespace SqlDbProjTasks {
    internal class FileInfo {
        private static readonly Regex FileIndexRegex = new Regex(@"[^\d]*(?<index>\d{1,6})[^\d]*", RegexOptions.Compiled);

        public string FilePath { get; set; }
        public int Index { get; set; }

        public static int ExtractFileIndex(string filePath, bool allowEmptyIndex) {
            var fileName = Path.GetFileName(filePath);
            var match = FileIndexRegex.Match(fileName);
            if(!match.Success && !allowEmptyIndex) {
                throw new InvalidOperationException(
                    string.Format("Couldn't extract file index information for file '{0}'. See task documentation for more info.", filePath)
                );
            }

            return match.Success ? int.Parse(match.Groups["index"].Value) : int.MaxValue;
        }

        public FileInfo(string path, bool allowEmptyIndex) {
            FilePath = path;
            Index = ExtractFileIndex(path, allowEmptyIndex);
        }
    }
}
