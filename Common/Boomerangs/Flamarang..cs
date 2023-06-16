using System.Collections.Generic;
using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs;

public class FlamarangGlobalItem : GlobalItem
{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Flamarang && ServerConfig.Instance.VanillaChanges;

	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		int index = tooltips.FindLastIndex(t => t.Mod == "Terraria");
		tooltips.Insert(index + 1, new TooltipLine(Mod, "Tooltip0", "Spews molten sparks when it hits an enemy"));
	}
}

public class FlamarangGlobalProjectile : GlobalProjectile
{
	public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Flamarang && ServerConfig.Instance.VanillaChanges;

	public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) {
		int numSparks = Main.rand.Next(3, 5);
		for (int i = 0; i < numSparks; i++) {
			// TODO: Abstract this into its own dedicated projectile (wtf was i thinking)
			Projectile spark = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ProjectileID.WandOfSparkingSpark, projectile.damage / 3, 0f, projectile.owner);
			Vector2 velocity = new(0f, -1f);
			velocity = velocity.RotatedByRandom(MathHelper.TwoPi);
			velocity *= Main.rand.NextFloat(1.5f, 3.5f);
			spark.velocity = velocity;
			spark.DamageType = DamageClass.Melee;
			spark.usesLocalNPCImmunity = true;
			spark.localNPCImmunity[target.whoAmI] = 10;
			spark.extraUpdates = 1;
		}
	}
}