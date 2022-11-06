using Bangarang.Common.Configs;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs {
    public class CombatWrenchGI : GlobalItem {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.CombatWrench && ServerConfig.Instance.VanillaChanges;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            int index = tooltips.FindLastIndex(t => t.Mod == "Terraria");
            tooltips.Insert(index + 1, new(Mod, "Tooltip0", "Deals more damage when returning to you"));
        }
    }

    public class CombatWrenchGP : GlobalProjectile {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.CombatWrench && ServerConfig.Instance.VanillaChanges;

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (projectile.ai[0] == 1f) {
                damage = (int)((float)damage * 1.2f);
            }
        }
    }

    public class CombatWrenchGNPC : GlobalNPC {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Mechanic && ServerConfig.Instance.VanillaChanges;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (var rule in npcLoot.Get()) {
                if (rule is CommonDrop drop && drop.itemId == ItemID.CombatWrench) {
                    drop.chanceDenominator = 1;
                }
            }
        }
    }
}
