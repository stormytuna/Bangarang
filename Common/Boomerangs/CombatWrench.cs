using System.Collections.Generic;
using Bangarang.Common.Configs;
using Bangarang.Helpers;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs;

public class CombatWrenchGlobalItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.CombatWrench && ServerConfig.Instance.VanillaChanges;

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) => tooltips.InsertTooltip(new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.Bangarang.Items.CombatWrench.Tooltip0")), "Material");
}

public class CombatWrenchGlobalProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.CombatWrench && ServerConfig.Instance.VanillaChanges;

    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
        if (projectile.ai[0] == 1f) { // Happens when returning
            modifiers.FinalDamage += 0.2f;
        }
    }

    public class CombatWrenchGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Mechanic && ServerConfig.Instance.VanillaChanges;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            foreach (IItemDropRule rule in npcLoot.Get()) {
                if (rule is CommonDrop { itemId: ItemID.CombatWrench } drop) {
                    drop.chanceDenominator = 1;
                }
            }
        }
    }
}