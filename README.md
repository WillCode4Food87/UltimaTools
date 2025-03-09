🐉 Ultima Online Creature Parser
A work-in-progress tool for extracting and analyzing creature data from Ultima Online servers.

📌 Features
✅ Parses creature .cs script files to extract:

AI Type, Fight Mode, Control Slots
Taming Skill Requirements
Combat Stats (HP, Damage, Armor)
Damage Types & Resistances
Skill Levels
Tamability
✅ Generates a CSV file (CreatureData.csv) for balance analysis.

✅ Helps identify overpowered/underpowered creatures by comparing control slots vs combat stats.

🚀 How It Works
Parses all .cs files in a configurable target directory (default: Scripts/Mobiles/Animals) maybe?.
Extracts relevant creature attributes using Roslyn SyntaxTree parsing.
Outputs a structured CSV file that can be analyzed in Excel, Python, or other tools.

🔧 Changing the Target Directory
By default, the parser scans:
Scripts/Mobiles/Animals

To change this:
Open Program.cs.
Modify the baseDirectory variable:
string baseDirectory = Path.Combine(solutionDirectory, @"Scripts/Mobiles/Animals");
Change Scripts/Mobiles/Animals to your desired folder.

📂 Output
The CSV file (CreatureData.csv) is generated in:
[Solution Directory]/CreatureData.csv
This file contains every extracted creature, making it easy to analyze balance trends.

🛠️ Work In Progress
🚧 This project is actively being developed! Future improvements include:

Better handling of AI behavior parsing
GUI for easier configuration
Contributions & feedback are welcome! 🐉🔥

