using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public class BeemerangProj : Boomerang {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Beemerang");
        }

        public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

        public override void SetDefaults() {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 14f;
            HomingOnOwnerStrength = 1.5f;
            TravelOutFrames = 30;
            DoTurn = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            int numBees = Main.rand.Next(3);
            for (int i = 0; i < numBees; i++) {
                Vector2 velocity = new(Main.rand.NextFloat(0f, 0.3f), 0f);
                velocity = velocity.RotatedByRandom(MathHelper.TwoPi);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, Owner.beeType(), Owner.beeDamage(damage), Owner.beeKB(knockback), Projectile.owner);
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}