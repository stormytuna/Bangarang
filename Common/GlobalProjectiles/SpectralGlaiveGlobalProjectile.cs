using System.Linq;
using Bangarang.Common.Systems;
using Bangarang.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Bangarang.Common.Players;

public class SpectralGlaiveGlobalProjectile : GlobalProjectile
{
	public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => BoomerangInfoSystem.ProjectilesThatAreBoomerangs.Contains(entity.type);

	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (Main.player[projectile.owner].GetModPlayer<BangarangPlayer>().BoomerangSpectralGlaives) {
			Projectile spectralGlaive = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpectralGlaiveProj>(), projectile.damage / 5, 0f, projectile.owner, projectile.whoAmI, MathHelper.Pi);
			spectralGlaive.extraUpdates = projectile.extraUpdates;
			spectralGlaive.DamageType = projectile.DamageType;
			spectralGlaive = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpectralGlaiveProj>(), projectile.damage / 5, 0f, projectile.owner, projectile.whoAmI);
			spectralGlaive.extraUpdates = projectile.extraUpdates;
			spectralGlaive.DamageType = projectile.DamageType;
		}
	}
}