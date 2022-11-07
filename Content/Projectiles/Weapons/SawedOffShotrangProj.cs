using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public class SawedOffShotrangProj : Boomerang {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Sawed-off Shotrang");
        }

        public override void SetDefaults() {
            Projectile.width = 38;
            Projectile.height = 14;
            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 14f;
            HomingOnOwnerStrength = 4f;
            TravelOutFrames = 30;
            Rotation = 0.35f;
            DoTurn = true;
        }

        public override void OnReachedApex() => DoShotgunBlast();

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (Projectile.ai[0] == 0f) {
                DoShotgunBlast();
            }

            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if (Projectile.ai[0] == 0f) {
                DoShotgunBlast(target.whoAmI);
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        private void DoShotgunBlast(int whoAmI = -1) {
            for (int i = 0; i < 4; i++) {
                Vector2 velocity = Projectile.velocity;
                velocity = velocity.RotatedByRandom(MathHelper.PiOver4);
                velocity *= 8f;
                Projectile bullet = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ProjectileID.Bullet, Projectile.damage / 3, Projectile.knockBack / 3f, Projectile.owner);
                bullet.DamageType = DamageClass.Melee;
                if (whoAmI != -1) {
                    bullet.usesLocalNPCImmunity = true;
                    bullet.localNPCImmunity[whoAmI] = 10;
                }
                SoundEngine.PlaySound(SoundID.Item36, Projectile.Center);
            }
        }
    }
}