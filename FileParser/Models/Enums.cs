using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Models
{
    public enum AIType
    {
        AI_Use_Default,
        AI_Melee,
        AI_Animal,
        AI_Archer,
        AI_Healer,
        AI_Vendor,
        AI_Mage,
        AI_Berserk,
        AI_Predator,
        AI_Thief,
        AI_Citizen
    }
    public enum FightMode
    {
        None,           // Never focus on others
        Aggressor,      // Only attack aggressors
        Strongest,      // Attack the strongest
        Weakest,        // Attack the weakest
        Closest,        // Attack the closest
        Evil,           // Only attack aggressor -or- negative karma
        Good,           // Only attack aggressor -or- positive karma
        CharmMonster,
        CharmAnimal
    }

    [Flags]
    public enum FoodType
    {
        None = 0x0000,
        Meat = 0x0001,
        FruitsAndVegies = 0x0002,
        GrainsAndHay = 0x0004,
        Fish = 0x0008,
        Eggs = 0x0010,
        Gold = 0x0020,
        Fire = 0x0040,
        Gems = 0x0080,
        Nox = 0x0100,
        Sea = 0x0200,
        Moon = 0x0400
    }

    [Flags]
    public enum PackInstinct
    {
        None = 0x0000,
        Canine = 0x0001,
        Ostard = 0x0002,
        Feline = 0x0004,
        Arachnid = 0x0008,
        Daemon = 0x0010,
        Bear = 0x0020,
        Equine = 0x0040,
        Bull = 0x0080
    }

    public enum MeatType
    {
        Ribs,
        Bird,
        LambLeg,
        Fish,
        Pigs
    }

    public enum ClothType
    {
        Fabric,
        Furry,
        Wooly,
        Silk,
        Haunted,
        Arctic,
        Pyre,
        Venomous,
        Mysterious,
        Vile,
        Divine,
        Fiendish
    }

    public enum ScaleType
    {
        Red,
        Yellow,
        Black,
        Green,
        White,
        Blue,
        Dinosaur,
        Metallic,
        Brazen,
        Umber,
        Violet,
        Platinum,
        Cadalyte,
        SciFi
    }

    public enum SkeletalType
    {
        Brittle,
        Drow,
        Orc,
        Reptile,
        Ogre,
        Troll,
        Gargoyle,
        Minotaur,
        Lycan,
        Shark,
        Colossal,
        Mystical,
        Vampire,
        Lich,
        Sphinx,
        Devil,
        Draco,
        Xeno,
        All,
        SciFi
    }

    public enum HideType
    {
        Regular,
        Spined,
        Horned,
        Barbed,
        Necrotic,
        Volcanic,
        Frozen,
        Goliath,
        Draconic,
        Hellish,
        Dinosaur,
        Alien
    }

    public enum SkinType
    {
        Demon,
        Dragon,
        Nightmare,
        Snake,
        Troll,
        Unicorn,
        Icy,
        Lava,
        Seaweed,
        Dead
    }

    public enum GraniteType
    {
        Iron,
        DullCopper,
        ShadowIron,
        Copper,
        Bronze,
        Gold,
        Agapite,
        Verite,
        Valorite,
        Nepturite,
        Obsidian,
        Mithril,
        Xormite,
        Dwarven,
        Steel,
        Brass
    }

    public enum RockType
    {
        Iron,
        DullCopper,
        ShadowIron,
        Copper,
        Bronze,
        Gold,
        Agapite,
        Verite,
        Valorite,
        Nepturite,
        Obsidian,
        Steel,
        Brass,
        Mithril,
        Xormite,
        Dwarven,
        Amethyst,
        Emerald,
        Garnet,
        Ice,
        Jade,
        Marble,
        Onyx,
        Quartz,
        Ruby,
        Sapphire,
        Silver,
        Spinel,
        StarRuby,
        Topaz,
        Caddellite,
        Crystals,
        Stones,
        SciFi
    }

    public enum MetalType
    {
        Iron,
        DullCopper,
        ShadowIron,
        Copper,
        Bronze,
        Gold,
        Agapite,
        Verite,
        Valorite,
        Nepturite,
        Obsidian,
        Steel,
        Brass,
        Mithril,
        Xormite,
        Dwarven,
        SciFi
    }

    public enum WoodType
    {
        Regular,
        Ash,
        Cherry,
        Ebony,
        GoldenOak,
        Hickory,
        Mahogany,
        Oak,
        Pine,
        Ghost,
        Rosewood,
        Walnut,
        Petrified,
        Driftwood,
        Elven
    }

    public enum SkillName
    {
        Alchemy = 0,
        Anatomy = 1,
        Druidism = 2,
        Mercantile = 3,
        ArmsLore = 4,
        Parry = 5,
        Begging = 6,
        Blacksmith = 7,
        Bowcraft = 8,
        Peacemaking = 9,
        Camping = 10,
        Carpentry = 11,
        Cartography = 12,
        Cooking = 13,
        Searching = 14,
        Discordance = 15,
        Psychology = 16,
        Healing = 17,
        Seafaring = 18,
        Forensics = 19,
        Herding = 20,
        Hiding = 21,
        Provocation = 22,
        Inscribe = 23,
        Lockpicking = 24,
        Magery = 25,
        MagicResist = 26,
        Tactics = 27,
        Snooping = 28,
        Musicianship = 29,
        Poisoning = 30,
        Marksmanship = 31,
        Spiritualism = 32,
        Stealing = 33,
        Tailoring = 34,
        Taming = 35,
        Tasting = 36,
        Tinkering = 37,
        Tracking = 38,
        Veterinary = 39,
        Swords = 40,
        Bludgeoning = 41,
        Fencing = 42,
        FistFighting = 43,
        Lumberjacking = 44,
        Mining = 45,
        Meditation = 46,
        Stealth = 47,
        RemoveTrap = 48,
        Necromancy = 49,
        Focus = 50,
        Knightship = 51,
        Bushido = 52,
        Ninjitsu = 53,
        Elementalism = 54,
        Mysticism = 55,
        Imbuing = 56,
        Throwing = 57
    }
}
