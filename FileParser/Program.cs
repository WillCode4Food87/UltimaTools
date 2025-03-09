using FileParser.Models;
using FileParser.Services.Export;
using FileParser.Services.Parse;
using Microsoft.Extensions.DependencyInjection;

namespace FileParser
{
    class Program
    {
        static void Main()
        {
            var serviceProvider = new ServiceCollection()
           .AddSingleton(typeof(IExporter<>), typeof(CsvExporter<>))
           .AddSingleton(typeof(IParser<CreatureData>), typeof(BaseCreatureParser))
           .BuildServiceProvider();

            string binDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string solutionDirectory = Path.GetFullPath(Path.Combine(binDirectory, @"..\..\..\.."));

            // This can target any mobiles directory, It is currently set to target this 
            string baseDirectory = Path.Combine(solutionDirectory, @"Scripts\Mobiles");

            string outputPath = Path.Combine(solutionDirectory, "CreatureData.csv");

            if (!Directory.Exists(baseDirectory))
            {
                Console.WriteLine($"Error: The directory '{baseDirectory}' does not exist.");
                return;
            }

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            // Scan for all .cs files if directory exists
            var csFiles = Directory.GetFiles(baseDirectory, "*.cs", SearchOption.AllDirectories)
                        .Where(file => file.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                        .ToArray();

            var creatureParser = serviceProvider.GetRequiredService<IParser<CreatureData>>();
            List<CreatureData> creatures = creatureParser.Parse(csFiles);

            var creatureExporter = serviceProvider.GetRequiredService<IExporter<CreatureData>>();
            creatureExporter.Export(creatures, outputPath);
            Console.WriteLine($"Extraction complete. {creatures.Count} creatures found. Output written to {outputPath}");
        }
    }
}
