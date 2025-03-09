
namespace FileParser.Models
{
    public class CreatureData
    {
        public string? ClassName { get; set; }
        public string? FilePath { get; set; }
        public string? FolderGroup { get; set; }

        // Base Constructor Parameters
        public AIType? AIType { get; set; }
        public FightMode? FightMode { get; set; }
        public int? IRangePerception { get; set; }
        public int? IRangeFight { get; set; }
        public double? DActiveSpeed { get; set; }
        public double? DPassiveSpeed { get; set; }

        // Enum properties (single-value)
        public HideType? HideType { get; set; }
        public SkinType? SkinType { get; set; }

        // Multi-value enum (bitwise integer)
        public FoodType FavoriteFood { get; set; } = FoodType.None;

        // Converted enums
        public string FavoriteFoodString { get; set; } = "None";

        // Standard numeric attributes
        public int? ControlSlots { get; set; }
        public double? MinTameSkill { get; set; }
        public int? Fame { get; set; }
        public int? Karma { get; set; }
        public int? VirtualArmor { get; set; }
        public bool Tamable { get; set; } = false;
        public int? HitsMin { get; set; }
        public int? HitsMax { get; set; }
        public int? StamMin { get; set; }
        public int? StamMax { get; set; }
        public int? ManaMin { get; set; }
        public int? ManaMax { get; set; }
        public int? DamageMin { get; set; }
        public int? DamageMax { get; set; }
        public int? StrMin { get; set; }
        public int? StrMax { get; set; }
        public int? DexMin { get; set; }
        public int? DexMax { get; set; }
        public int? IntMin { get; set; }
        public int? IntMax { get; set; }
        public int DamageTypePhysical { get; set; } = 0;
        public int DamageTypeFire { get; set; } = 0;
        public int DamageTypeCold { get; set; } = 0;
        public int DamageTypePoison { get; set; } = 0;
        public int DamageTypeEnergy { get; set; } = 0;
        public int ResistPhysicalMin { get; set; } = 0;
        public int ResistPhysicalMax { get; set; } = 0;
        public int ResistFireMin { get; set; } = 0;
        public int ResistFireMax { get; set; } = 0;
        public int ResistColdMin { get; set; } = 0;
        public int ResistColdMax { get; set; } = 0;
        public int ResistPoisonMin { get; set; } = 0;
        public int ResistPoisonMax { get; set; } = 0;
        public int ResistEnergyMin { get; set; } = 0;
        public int ResistEnergyMax { get; set; } = 0;
        public Dictionary<SkillName, double> SkillsMin { get; set; } = [];
        public Dictionary<SkillName, double> SkillsMax { get; set; } = [];

        // Flag indicating additional complexity that may require special handling (Currently removed)
        public bool IsComplex { get; set; } = false;
    }

}
