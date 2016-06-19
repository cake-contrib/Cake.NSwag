using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;

namespace Cake.NSwag
{
    internal static class CoreExtensions
    {
        internal static void WriteContent(this IFileSystem fileSystem, FilePath path, string content)
        {
            using (var writer = new StreamWriter(fileSystem.GetFile(path).OpenWrite()))
            {
                writer.Write(content);
            }
        }

        internal static string ReadContent(this IFileSystem fileSystem, FilePath path)
        {
            using (var reader = new StreamReader(fileSystem.GetFile(path).OpenRead()))
            {
                return reader.ReadToEnd();
            }
        }

        internal static KeyValuePair<string, string> SplitClassPath(this string s)
        {
            var segments = s.Split('.');
            return new KeyValuePair<string, string>(string.Join(".", segments.Take(segments.Length - 1)), segments.Last());
        }
    }
}
