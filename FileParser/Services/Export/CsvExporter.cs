
using FileParser.Models;
using System.Reflection;

namespace FileParser.Services.Export
{
    public class CsvExporter<T> : IExporter<T> where T : new()
    {
        public void Export(List<T> data, string outputPath)
        {
            if (data == null || data.Count == 0)
            {
                Console.WriteLine("No data to export.");
                return;
            }

            using (StreamWriter sw = new(outputPath))
            {
                // Extract properties dynamically (column headers)
                var properties = typeof(T).GetProperties();
                List<string> csvHeaders = [.. properties.Select(p => p.Name)];

                // Check if T has "SkillsMin" and "SkillsMax" properties
                // Add skill columns dynamically if the object has skills
                PropertyInfo? skillsMinProperty = typeof(T).GetProperty("SkillsMin");
                PropertyInfo? skillsMaxProperty = typeof(T).GetProperty("SkillsMax");

                if (skillsMinProperty != null && skillsMaxProperty != null)
                {
                    foreach (SkillName skill in Enum.GetValues(typeof(SkillName)))
                    {
                        csvHeaders.Add($"{skill}_Min");
                        csvHeaders.Add($"{skill}_Max");
                    }
                }

                if (skillsMinProperty != null && skillsMaxProperty != null)
                {
                    foreach (SkillName skill in Enum.GetValues(typeof(SkillName)))
                    {
                        csvHeaders.Add($"{skill}_Min");
                        csvHeaders.Add($"{skill}_Max");
                    }
                }

                // Write header row
                sw.WriteLine(string.Join(",", csvHeaders));

                // Write data rows dynamically
                foreach (var item in data)
                {
                    List<string> rowData = [.. properties.Select(p => EscapeCsv(p.GetValue(item)?.ToString() ?? ""))];

                    // Add skill min/max values dynamically
                    if (skillsMinProperty?.GetValue(item) is Dictionary<SkillName, double> skillsMinDict &&
                        skillsMaxProperty?.GetValue(item) is Dictionary<SkillName, double> skillsMaxDict)
                    {
                        foreach (SkillName skill in Enum.GetValues<SkillName>())
                        {
                            rowData.Add(skillsMinDict.TryGetValue(skill, out double minValue) ? minValue.ToString("F1") : "0.0");
                            rowData.Add(skillsMaxDict.TryGetValue(skill, out double maxValue) ? maxValue.ToString("F1") : "0.0");
                        }
                    }


                    sw.WriteLine(string.Join(",", rowData));
                }
            }

            Console.WriteLine($"CSV Export completed: {outputPath}");
        }

        // Ensures proper CSV formatting
        private static string EscapeCsv(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";

            if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
            {
                field = field.Replace("\"", "\"\""); // Escape existing quotes
                return "\"" + field + "\"";
            }

            return field;
        }
    }
}

