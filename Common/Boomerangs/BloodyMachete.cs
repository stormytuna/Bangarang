using System.Collections.Generic;
using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs;

public class BloodyMacheteGlobalItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.BloodyMachete && ServerConfig.Instance.VanillaChanges;

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
        int index = tooltips.FindLastIndex(t => t.Mod == "Terraria");
        // TODO: Make these localised!
        tooltips.Insert(index + 1, new TooltipLine(Mod, "Tooltip0", "Increases in damage while airborne"));
        tooltips.Insert(index + 2, new TooltipLine(Mod, "Tooltip1", "'Go, do a crime'"));
    }
}

public class BloodyMacheteGlobalProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.BloodyMachete && ServerConfig.Instance.VanillaChanges;

    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
        // Bloody Machete uses ai[1] as a frame counter in vanilla so we can just reuse that
        // Should get us a value between 0 and 1 when the frame counter is between 30 and 60
        float lerpAmount = (MathHelper.Clamp(projectile.ai[1], 30f, 60f) - 30f) / 30f;
        float damageIncrease = MathHelper.Lerp(0f, 0.5f, lerpAmount);
        modifiers.FinalDamage += damageIncrease;
    }
}

public class BloodyMacheteGlobalNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.SkeletonMerchant && ServerConfig.Instance.VanillaChanges;

    public override void ModifyShop(NPCShop shop) => shop.Add(ItemID.BloodyMachete, Condition.MoonPhasesHalf1);
}