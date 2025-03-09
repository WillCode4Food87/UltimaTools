using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Custom.KoperPets
{
    public static class KoperDruidismGain
    {
        private static readonly Dictionary<Mobile, DateTime> _cooldowns = new Dictionary<Mobile, DateTime>();
        private static readonly TimeSpan CooldownTime = TimeSpan.FromSeconds(MyServerSettings.KoperCooldown()); // 20-second cooldown

        private static readonly string[] SuccessMessages = new string[]
        {
            "Yoour knowledge of animals has increased.",
            "Your understanding of animal behavior improves."
        };

        public static void TryGainDruidismSkill(Mobile owner)
        {
            if (owner == null || !owner.Alive || !MyServerSettings.KoperPets())
                return; // No skill gain for dead players/system disabled

            // Check if the player is on cooldown
            if (_cooldowns.ContainsKey(owner) && DateTime.UtcNow < _cooldowns[owner])
            {
                return; // Cooldown is active, exit without giving skill
            }

            double herdingSkill = owner.Skills[SkillName.Herding].Base;
            double gainChance;
            double minGain;
            double maxGain;
            double herdingMultiplier = MyServerSettings.KoperHerdingChance();


            // Determine gain chance and amount based on skill level
            if (herdingMultiplier <= 0) herdingMultiplier = 1.0; // Ensure valid value
            if (herdingSkill <= 30.0) { gainChance = 0.20 * herdingMultiplier; minGain = 0.1; maxGain = 1.0; }
            else if (herdingSkill <= 50.0) { gainChance = 0.15 * herdingMultiplier; minGain = 0.1; maxGain = 0.5; }
            else if (herdingSkill <= 70.0) { gainChance = 0.10 * herdingMultiplier; minGain = 0.1; maxGain = 0.2; }
            else if (herdingSkill <= 100.0) { gainChance = 0.05 * herdingMultiplier; minGain = 0.1; maxGain = 0.1; }
            else if (herdingSkill < 125.0) { gainChance = 0.02 * herdingMultiplier; minGain = 0.1; maxGain = 0.1; }
            else return; // No gain if at max skill

            if (Utility.RandomDouble() <= gainChance)
            {
                double skillGain = Utility.RandomDouble() * (maxGain - minGain) + minGain;
                owner.Skills[SkillName.Druidism].Base += skillGain;

                // Select a random message for variety
                if (MyServerSettings.KoperPetsImmersive())
                {
                    owner.SendMessage(SuccessMessages[Utility.Random(SuccessMessages.Length)]);
                }
                // Start cooldown timer
                _cooldowns[owner] = DateTime.UtcNow + CooldownTime;
            }
        }
    }
}
