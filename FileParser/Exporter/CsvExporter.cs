using FileParser.Models;

namespace FileParser.Exporter
{
    public static class CsvExporter
    {
        public static void Export(List<CreatureData> creatures, string outputCsv)
        {
            using (StreamWriter sw = new StreamWriter(outputCsv))
            {
                List<string> csvHeaders = new List<string>
            {
                "ClassName",
                "FilePath",
                "FolderGroup",
                "AIType",
                "FightMode",
                "RangePerception",
                "RangeFight",
                "ActiveSpeed",
                "PassiveSpeed",
                "ControlSlots",
                "MinTameSkill",
                "Fame",
                "Karma",
                "VirtualArmor",
                "Tamable",
                "HitsMin",
                "HitsMax",
                "StamMin",
                "StamMax",
                "ManaMin",
                "ManaMax",
                "DamageMin",
                "DamageMax",
                "StrMin",
                "StrMax",
                "DexMin",
                "DexMax",
                "IntMin",
                "IntMax",
                "DamageTypePhysical",
                "DamageTypeFire",
                "DamageTypeCold",
                "DamageTypePoison",
                "DamageTypeEnergy",
                "ResistPhysicalMin",
                "ResistPhysicalMax",
                "ResistFireMin",
                "ResistFireMax",
                "ResistColdMin",
                "ResistColdMax",
                "ResistPoisonMin",
                "ResistPoisonMax",
                "ResistEnergyMin",
                "ResistEnergyMax",
                "FavoriteFood",
                "HideType",
                "SkinType",
            };

                // ✅ Add skill columns dynamically (Skill_X_Min, Skill_X_Max)
                foreach (SkillName skill in Enum.GetValues(typeof(SkillName)))
                {
                    csvHeaders.Add($"{skill}_Min");
                    csvHeaders.Add($"{skill}_Max");
                }

                // ✅ Write header row
                sw.WriteLine(string.Join(",", csvHeaders));

                // ✅ Write data rows
                foreach (var d in creatures)
                {
                    List<string> rowData = new List<string>
                    {
                        EscapeCsv(d.ClassName),
                        EscapeCsv(d.FilePath),
                        EscapeCsv(d.FolderGroup),
                        d.AIType?.ToString() ?? "",
                        d.FightMode?.ToString() ?? "",
                        d.iRangePerception?.ToString() ?? "",
                        d.iRangeFight?.ToString() ?? "",
                        d.dActiveSpeed?.ToString() ?? "",
                        d.dPassiveSpeed?.ToString() ?? "",
                        d.ControlSlots?.ToString() ?? "",
                        d.MinTameSkill?.ToString() ?? "",
                        d.Fame?.ToString() ?? "",
                        d.Karma?.ToString() ?? "",
                        d.VirtualArmor?.ToString() ?? "",
                        d.Tamable.ToString(),
                        d.HitsMin?.ToString() ?? "",
                        d.HitsMax?.ToString() ?? "",
                        d.StamMin?.ToString() ?? "",
                        d.StamMax?.ToString() ?? "",
                        d.ManaMin?.ToString() ?? "",
                        d.ManaMax?.ToString() ?? "",
                        d.DamageMin?.ToString() ?? "",
                        d.DamageMax?.ToString() ?? "",
                        d.StrMin?.ToString() ?? "",
                        d.StrMax?.ToString() ?? "",
                        d.DexMin?.ToString() ?? "",
                        d.DexMax?.ToString() ?? "",
                        d.IntMin?.ToString() ?? "",
                        d.IntMax?.ToString() ?? "",
                        d.DamageTypePhysical.ToString(),
                        d.DamageTypeFire.ToString(),
                        d.DamageTypeCold.ToString(),
                        d.DamageTypePoison.ToString(),
                        d.DamageTypeEnergy.ToString(),
                        d.ResistPhysicalMin.ToString(),
                        d.ResistPhysicalMax.ToString(),
                        d.ResistFireMin.ToString(),
                        d.ResistFireMax.ToString(),
                        d.ResistColdMin.ToString(),
                        d.ResistColdMax.ToString(),
                        d.ResistPoisonMin.ToString(),
                        d.ResistPoisonMax.ToString(),
                        d.ResistEnergyMin.ToString(),
                        d.ResistEnergyMax.ToString(),
                        EscapeCsv(d.FavoriteFoodString ?? ""),
                        d.HideType?.ToString() ?? "",
                        d.SkinType?.ToString() ?? ""
                    };

                    // ✅ Add skill min/max values dynamically
                    foreach (SkillName skill in Enum.GetValues(typeof(SkillName)))
                    {
                        rowData.Add(d.SkillsMin.ContainsKey(skill) ? d.SkillsMin[skill].ToString("F1") : "0.0");
                        rowData.Add(d.SkillsMax.ContainsKey(skill) ? d.SkillsMax[skill].ToString("F1") : "0.0");
                    }

                    sw.WriteLine(string.Join(",", rowData));
                }
            }
        }

        private static string EscapeCsv(string? field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                field = field.Replace("\"", "\"\"");
                return "\"" + field + "\"";
            }
            return field;
        }
    }
}
