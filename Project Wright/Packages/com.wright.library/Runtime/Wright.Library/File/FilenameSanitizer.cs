using System.Text.RegularExpressions;

namespace Wright.Library.File
{
    public static class FilenameSanitizer
    {
        public static string Sanitize(string filename)
        {
            const string pattern = @"[^\d\w\.]";

            return Regex.Replace(filename, pattern, "_");
        }
    }
}