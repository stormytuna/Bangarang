using System.Collections.Generic;
using Bangarang.Common.Configs;
using Bangarang.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs;

public class ShroomerangGlobalItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Shroomerang && ServerConfig.Instance.VanillaChanges;

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) => tooltips.InsertTooltip(new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.Bangarang.Items.Shroomerang.Tooltip0")), "Material");
}

public class ShroomerangGlobalProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Shroomerang && ServerConfig.Instance.VanillaChanges;

    public override void AI(Projectile projectile) {
        // Vanilla uses ai[1] as a frame counter so we can too
        if (projectile.ai[1] % 15 == 11 && Main.myPlayer == projectile.owner) {
            // Change in position just centres the shroom on the boomerangs centre
            Projectile shroom = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center - new Vector2(11f, 11f), Vector2.Zero, ProjectileID.Mushroom, projectile.damage / 3, 0f, projectile.owner);
            shroom.alpha = 80;
        }

        // Keep using ai[1] as a frame counter while flying back
        if (projectile.ai[0] != 0) {
            projectile.ai[1]++;
        }
    }
}