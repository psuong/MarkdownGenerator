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

            foreach (var g in types.GroupBy(x => x.Namespace).OrderBy(x => x.Key)) {
                if (!Directory.Exists(dest)) { Directory.CreateDirectory(dest); }
                
                // Make the namespace a directory
                var namespaceDirectory = Path.Combine(dest, g.Key);

                if (!Directory.Exists(namespaceDirectory)) { Directory.CreateDirectory(namespaceDirectory); }
                
                homeBuilder.HeaderWithLink(2, g.Key, g.Key);
                homeBuilder.AppendLine();

                var sb = new StringBuilder();
                foreach (var classItem in g.OrderBy(x => x.Name)) {
                    // Make the class a directory
                    var classDirectory = Path.Combine(namespaceDirectory, classItem.BeautifyName.Replace("<", "-").Replace(">", "").Replace(",", "").Replace(" ", "-").ToLower());

                    if (!Directory.Exists(classDirectory)) { Directory.CreateDirectory(classDirectory); }
                    
                    // Write the contents of the class to the file
                    var file = Path.Combine(classDirectory, "_index.md");
                    Console.WriteLine("Making: " + file);
                    File.WriteAllText(file, classItem.ToString());

                }

            }

            // Gen Home
            File.WriteAllText(Path.Combine(dest, "_index.md"), homeBuilder.ToString());
        }
    }
}
