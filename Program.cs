using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MarkdownWikiGenerator {
    class Program {
        // 0 = dll src path, 1 = dest root
        static void Main(string[] args) {
            // put dll & xml on same diretory.
            var target = "UniRx.dll"; // :)
            string dest = "md";
            if (args.Length == 1) {
                target = args[0];
            }
            else if (args.Length == 2) {
                target = args[0];
                dest = args[1];
            }

            var types = MarkdownGenerator.Load(target);

            // Home Markdown Builder
            var homeBuilder = new MarkdownBuilder();
            homeBuilder.Header(1, "References");
            homeBuilder.AppendLine();

            var namespaceIndex = 1;

            foreach (var g in types.GroupBy(x => x.Namespace).OrderBy(x => x.Key)) {
                if (!Directory.Exists(dest)) { Directory.CreateDirectory(dest); }
                
                // Make the namespace a directory
                var namespaceDirectory = Path.Combine(dest, g.Key);

                if (!Directory.Exists(namespaceDirectory)) { Directory.CreateDirectory(namespaceDirectory); }

                var namespaceFile = Path.Combine(namespaceDirectory, "_index.md");
                var namespaceContent = WriteContent(g.Key, namespaceIndex, "");
                File.WriteAllText(namespaceFile, namespaceContent);
                
                homeBuilder.HeaderWithLink(2, g.Key, g.Key);
                homeBuilder.AppendLine();

                namespaceIndex++;
                var classIndex = 1;
                foreach (var classItem in g.OrderBy(x => x.Name)) {
                    // Make the class a directory
                    var classDirectory = Path.Combine(namespaceDirectory, classItem.BeautifyName.Replace("<", "-").Replace(">", "").Replace(",", "").Replace(" ", "-").ToLower());

                    if (!Directory.Exists(classDirectory)) { Directory.CreateDirectory(classDirectory); }
                    
                    // Write the contents of the class to the file
                    var classFile = Path.Combine(classDirectory, "_index.md");
                    Console.WriteLine("Making: " + classFile);
                    var classContent = WriteContent(classItem.BeautifyName, classIndex, classItem.ToString());
                    File.WriteAllText(classFile, classContent);

                    classIndex++;
                }

            }

            // Gen Home
            File.WriteAllText(Path.Combine(dest, "_index.md"), homeBuilder.ToString());
        }

        private static string WriteContent(string title, int index, string content) {
            var sb = new StringBuilder();
            sb.AppendLine("+++");
            sb.AppendLine(string.Format("title = \"{0}\"", title));
            sb.AppendLine(string.Format("weight = {0}", index));
            sb.AppendLine("+++");
            sb.AppendLine();
            sb.Append(content);
            return sb.ToString();
        }
    }
}
