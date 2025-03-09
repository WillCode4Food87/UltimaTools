using FileParser.Exporter;
using FileParser.Models;
using FileParser.Parser;

namespace FileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string binDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string solutionDirectory = Path.GetFullPath(Path.Combine(binDirectory, @"..\..\..\.."));

            // This can target any mobiles directory, It is currently set to target this 
            string baseDirectory = Path.Combine(solutionDirectory, @"Scripts\Mobiles");

            string outputCsv = Path.Combine(solutionDirectory, "CreatureData.csv");

            if (!Directory.Exists(baseDirectory))
            {
                Console.WriteLine($"Error: The directory '{baseDirectory}' does not exist.");
                return;
            }

            if (File.Exists(outputCsv))
            {
                File.Delete(outputCsv);
            }

            List<CreatureData> creatures = new List<CreatureData>();

            // Scan for all .cs files if directory exists
            var csFiles = Directory.GetFiles(baseDirectory, "*.cs", SearchOption.AllDirectories)
                        .Where(file => file.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                        .ToArray();
            ICreatureParser parser = new BaseCreatureParser();

            foreach (var file in csFiles)
            {
                CreatureData? data = parser.Parse(file);
                if (data != null)
                {
                    // Set FolderGroup relative to the base directory.
                    data.FolderGroup = Path.GetDirectoryName(file)?.Replace(baseDirectory, "").Trim(Path.DirectorySeparatorChar) ?? "";
                    creatures.Add(data);
                }
            }

            CsvExporter.Export(creatures, outputCsv);
            Console.WriteLine($"Extraction complete. {creatures.Count} creatures found. Output written to {outputCsv}");
        }
    }
}
