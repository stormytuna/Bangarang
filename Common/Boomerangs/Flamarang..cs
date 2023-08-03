using System.Collections.Generic;
using Bangarang.Common.Configs;
using Bangarang.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Microsoft.Xna.Framework.MathHelper;

namespace Bangarang.Common.Boomerangs;

public class FlamarangGlobalItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Flamarang && ServerConfig.Instance.VanillaChanges;

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) => tooltips.InsertTooltip(new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.Bangarang.Items.Flamarang.Tooltip0")), "Material");
}

public class FlamarangSpark : ModProjectile
{
    public override void SetDefaults() {
        Projectile.width = 10;
        Projectile.height = 10;
        Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
        AIType = ProjectileID.WandOfSparkingSpark;
        Projectile.alpha = 255;
        Projectile.extraUpdates = 1;

        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.penetrate = 2;
        Projectile.usesLocalNPCImmunity = true;
    }

    public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.WandOfSparkingSpark}";
}

public class FlamarangGlobalProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Flamarang && ServerConfig.Instance.VanillaChanges;

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
        int numSparks = Main.rand.Next(3, 5);
        for (int i = 0; i < numSparks; i++) {
            Projectile spark = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<FlamarangSpark>(), projectile.damage / 3, 0f, projectile.owner);
            spark.velocity = Vector2.UnitX.RotatedByRandom(TwoPi) * Main.rand.NextFloat(1.5f, 3.5f);
            spark.localNPCImmunity[target.whoAmI] = 10;
        }
    }
}