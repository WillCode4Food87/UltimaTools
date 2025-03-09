using FileParser.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace FileParser.Parser
{
    public class BaseCreatureParser : ICreatureParser
    {
        public CreatureData? Parse(string filePath)
        {
            Console.WriteLine($"Parsing file: {filePath}");
            if (!filePath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Skipping non-CS file: {filePath}");
                return null;
            }


            string content = File.ReadAllText(filePath);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            var root = tree.GetRoot();

            var classDeclaration = root.DescendantNodes()
                                       .OfType<ClassDeclarationSyntax>()
                                       .FirstOrDefault(c => c.BaseList?.Types.Any(t => t.ToString().Contains("BaseCreature")) == true);

            if (classDeclaration == null)
                return null;

            CreatureData data = new CreatureData
            {
                ClassName = classDeclaration.Identifier.Text,
                FilePath = filePath,
                FolderGroup = Path.GetDirectoryName(filePath)
            };

            // Find the constructor and check if it calls base()
            var constructor = classDeclaration.DescendantNodes()
                                              .OfType<ConstructorDeclarationSyntax>()
                                              .FirstOrDefault();

            if (constructor != null && constructor.Initializer != null && constructor.Initializer.ThisOrBaseKeyword.Text == "base")
            {
                var baseArgs = constructor.Initializer.ArgumentList.Arguments.Select(a => a.ToString()).ToArray();
                if (baseArgs.Length >= 6)
                {
                    // Convert AIType and FightMode to Enums
                    data.AIType = ParseEnum<AIType>(baseArgs[0]);
                    data.FightMode = ParseEnum<FightMode>(baseArgs[1]);

                    // Parse numeric values
                    data.iRangePerception = ParseInt(baseArgs[2]);
                    data.iRangeFight = ParseInt(baseArgs[3]);
                    data.dActiveSpeed = ParseDouble(baseArgs[4]);
                    data.dPassiveSpeed = ParseDouble(baseArgs[5]);
                }
            }

            // Extract other properties
            SetStandardAssignedProperties(root, data);

            SetStats(root, data);

            SetDamage(root, data);

            SetResists(root, data);

            SetFavoriteFoods(root, classDeclaration, data);

            return data;
        }

        private static void SetStandardAssignedProperties(SyntaxNode root, CreatureData data)
        {
            foreach (var assignment in root.DescendantNodes().OfType<AssignmentExpressionSyntax>())
            {
                string left = assignment.Left.ToString();
                string right = assignment.Right.ToString();

                if (left.Contains("ControlSlots")) data.ControlSlots = ParseInt(right);
                else if (left.Contains("MinTameSkill")) data.MinTameSkill = ParseDouble(right);
                else if (left.Contains("Fame")) data.Fame = ParseInt(right);
                else if (left.Contains("Karma")) data.Karma = ParseInt(right);
                else if (left.Contains("VirtualArmor")) data.VirtualArmor = ParseInt(right);
                else if (left.Contains("Tamable")) data.Tamable = right.Trim().ToLower() == "true";
            }
        }

        private static void SetFavoriteFoods(SyntaxNode root, ClassDeclarationSyntax classDeclaration, CreatureData data)
        {
            foreach (SkillName skill in Enum.GetValues(typeof(SkillName)))
            {
                data.SkillsMin[skill] = 0.0;
                data.SkillsMax[skill] = 0.0;

            }
            // Extract `SetSkill(SkillName.X, min, max)`
            foreach (var invocation in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                string methodName = invocation.Expression.ToString();
                var args = invocation.ArgumentList.Arguments.Select(a => a.ToString()).ToArray();

                if (methodName == "SetSkill" && args.Length >= 2) // Handle both overloads
                {
                    string skillType = args[0].Replace("SkillName.", "").Trim();
                    double? minValue = ParseDouble(args[1]);
                    double? maxValue = (args.Length == 3) ? ParseDouble(args[2]) : minValue; // If single value, use for both

                    if (minValue.HasValue && maxValue.HasValue && Enum.TryParse(skillType, out SkillName parsedSkill))
                    {
                        data.SkillsMin[parsedSkill] = minValue.Value;
                        data.SkillsMax[parsedSkill] = maxValue.Value;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Unknown SkillName '{skillType}'");
                    }
                }
            }

            var favoriteFoodProperty = classDeclaration.DescendantNodes()
                                       .OfType<PropertyDeclarationSyntax>()
                                       .FirstOrDefault(p => p.Identifier.Text == "FavoriteFood");

            if (favoriteFoodProperty != null)
            {
                var getter = favoriteFoodProperty.DescendantNodes().OfType<ReturnStatementSyntax>().FirstOrDefault();
                if (getter != null)
                {
                    string foodExpression = getter.Expression.ToString();
                    data.FavoriteFoodString = ParseFoodTypeToString(foodExpression);
                }
            }
        }

        private static void SetResists(SyntaxNode root, CreatureData data)
        {
            // Extract `SetResistance(ResistanceType.X, min, max)`
            foreach (var invocation in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                string methodName = invocation.Expression.ToString();
                var args = invocation.ArgumentList.Arguments.Select(a => a.ToString()).ToArray();

                if (methodName == "SetResistance" && args.Length >= 2) // Handle both overloads
                {
                    string type = args[0].Replace("ResistanceType.", "").Trim();
                    int? minValue = ParseInt(args[1]);
                    int? maxValue = (args.Length == 3) ? ParseInt(args[2]) : minValue; // If single value, use for both

                    if (minValue.HasValue && maxValue.HasValue)
                    {
                        switch (type)
                        {
                            case "Physical": data.ResistPhysicalMin = minValue.Value; data.ResistPhysicalMax = maxValue.Value; break;
                            case "Fire": data.ResistFireMin = minValue.Value; data.ResistFireMax = maxValue.Value; break;
                            case "Cold": data.ResistColdMin = minValue.Value; data.ResistColdMax = maxValue.Value; break;
                            case "Poison": data.ResistPoisonMin = minValue.Value; data.ResistPoisonMax = maxValue.Value; break;
                            case "Energy": data.ResistEnergyMin = minValue.Value; data.ResistEnergyMax = maxValue.Value; break;
                            default:
                                Console.WriteLine($"Warning: Unknown ResistanceType '{type}'");
                                break;
                        }
                    }
                }
            }
        }

        private static void SetDamage(SyntaxNode root, CreatureData data)
        {
            // Extract `SetDamageType(ResistanceType.X, value)`
            foreach (var invocation in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                string methodName = invocation.Expression.ToString();
                var args = invocation.ArgumentList.Arguments.Select(a => a.ToString()).ToArray();

                if (methodName == "SetDamageType" && args.Length == 2)
                {
                    string type = args[0].Replace("ResistanceType.", "").Trim();
                    int? value = ParseInt(args[1]);

                    if (value.HasValue)
                    {
                        switch (type)
                        {
                            case "Physical": data.DamageTypePhysical = value.Value; break;
                            case "Fire": data.DamageTypeFire = value.Value; break;
                            case "Cold": data.DamageTypeCold = value.Value; break;
                            case "Poison": data.DamageTypePoison = value.Value; break;
                            case "Energy": data.DamageTypeEnergy = value.Value; break;
                            default:
                                Console.WriteLine($"Warning: Unknown DamageType '{type}'");
                                break;
                        }
                    }
                }
            }
        }

        private static void SetStats(SyntaxNode root, CreatureData data)
        {
            // Extract SetStats (SetHits, SetDamage, SetStr, SetDex, SetInt)
            foreach (var invocation in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                string methodName = invocation.Expression.ToString();
                var args = invocation.ArgumentList.Arguments.Select(a => a.ToString()).ToArray();

                if (args.Length == 1)
                {
                    int? value = ParseInt(args[0]);
                    if (value.HasValue)
                    {
                        if (methodName == "SetHits") { data.HitsMin = value; data.HitsMax = value; }
                        if (methodName == "SetStam") { data.StamMin = value; data.StamMax = value; }
                        if (methodName == "SetMana") { data.ManaMin = value; data.ManaMax = value; }
                        if (methodName == "SetDamage") { data.DamageMin = value; data.DamageMax = value; }
                        if (methodName == "SetStr") { data.StrMin = value; data.StrMax = value; }
                        if (methodName == "SetDex") { data.DexMin = value; data.DexMax = value; }
                        if (methodName == "SetInt") { data.IntMin = value; data.IntMax = value; }
                    }
                }
                else if (args.Length == 2)
                {
                    int? minValue = ParseInt(args[0]);
                    int? maxValue = ParseInt(args[1]);

                    if (methodName == "SetHits") { data.HitsMin = minValue; data.HitsMax = maxValue; }
                    if (methodName == "SetStam") { data.StamMin = minValue; data.StamMax = maxValue; }
                    if (methodName == "SetMana") { data.ManaMin = minValue; data.ManaMax = maxValue; }
                    if (methodName == "SetDamage") { data.DamageMin = minValue; data.DamageMax = maxValue; }
                    if (methodName == "SetStr") { data.StrMin = minValue; data.StrMax = maxValue; }
                    if (methodName == "SetDex") { data.DexMin = minValue; data.DexMax = maxValue; }
                    if (methodName == "SetInt") { data.IntMin = minValue; data.IntMax = maxValue; }
                }
            }
        }

        private static int? ParseInt(string value)
        {
            return int.TryParse(value, out int result) ? result : (int?)null;
        }

        private static double? ParseDouble(string value)
        {
            return double.TryParse(value, out double result) ? result : (double?)null;
        }

        private static T? ParseEnum<T>(string value) where T : struct, Enum
        {
            // Remove "AIType." or "FightMode." prefix if present
            value = value.Replace("AIType.", "").Replace("FightMode.", "").Trim();

            if (Enum.TryParse(value, out T result))
            {
                return result;
            }

            Console.WriteLine($"Warning: Unknown {typeof(T).Name} value '{value}' in {typeof(T).Name}. Defaulting to null.");
            return null;
        }

        private static string ParseFoodTypeToString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "None";

            List<string> foodList = new List<string>();

            // Remove "FoodType." prefixes and split multiple values
            var foodParts = value.Replace("FoodType.", "").Split('|', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in foodParts)
            {
                if (Enum.TryParse(part.Trim(), out FoodType parsedFood))
                {
                    foodList.Add(parsedFood.ToString()); // Store readable name
                }
                else
                {
                    Console.WriteLine($"Warning: Unknown FoodType '{part.Trim()}'");
                }
            }

            string result = string.Join(", ", foodList);

            return result;
        }
    }
}
