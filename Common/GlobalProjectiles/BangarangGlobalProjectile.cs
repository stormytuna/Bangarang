using Bangarang.Common.Systems;
using Bangarang.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Players {
    public class BangarangGlobalProjectile : GlobalProjectile {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => ArraySystem.ProjectilesThatAreBoomerangs.Contains(entity.type);

        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            if (Main.player[projectile.owner].GetModPlayer<BangarangPlayer>().BoomerangSpectralGlaives) {
                Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpectralGlaiveProj>(), projectile.damage / 5, 0f, projectile.owner, projectile.whoAmI, MathHelper.Pi);
                proj.extraUpdates = projectile.extraUpdates;
                proj = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpectralGlaiveProj>(), projectile.damage / 5, 0f, projectile.owner, projectile.whoAmI, 0f);
                proj.extraUpdates = projectile.extraUpdates;
            }
        }

        public override void PostAI(Projectile projectile) {
            var modPlayer = Main.player[projectile.owner].GetModPlayer<BangarangPlayer>();

            // Return faster
            if (modPlayer.BoomerangReturnSpeedMult > 0 && projectile.ai[0] == 1f) {
                projectile.position += projectile.velocity * modPlayer.BoomerangReturnSpeedMult;
            }

            // Glow and dust
            if (modPlayer.BoomerangGlowAndDust) {
                Lighting.AddLight(projectile.Center, TorchID.Purple);

                if (Main.rand.NextBool(10)) {
                    Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.UndergroundHallowedEnemies);
                    d.velocity *= 0.8f;
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            var modPlayer = Main.player[projectile.owner].GetModPlayer<BangarangPlayer>();
            knockback *= modPlayer.BoomerangKnockbackMult;
        }
    }
}