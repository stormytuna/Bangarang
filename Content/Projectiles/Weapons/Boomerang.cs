using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public abstract class Boomerang : ModProjectile {
        /// <summary>How quickly the boomerang will return to its owner</summary>
        public float ReturnSpeed { get; set; }
        /// <summary>How strong the boomerang will home in on its owner when returning</summary>
        public float HomingOnOwnerStrength { get; set; }
        /// <summary>How many frames the boomerang will travel away from the player for. Default = 30</summary>
        public int TravelOutFrames { get; set; }

        public Player Owner { get => Main.player[Projectile.owner]; }

        public override void AI() {
            // Funky sound
            if (Projectile.soundDelay == 0) {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            // Spinny :D
            Projectile.rotation += 0.4f * Projectile.direction;

            // AI state 1 - travelling away from player 
            if (Projectile.ai[0] == 0f) {
                // Increase our frame counter
                Projectile.ai[1] += 1f;
                // Check if our frame counter is high enough to turn around
                if (Projectile.ai[1] >= 30f) {
                    Projectile.ai[0] = 2f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }

                return; // So this part can acts as a guard clause
            }

            // AI state 2 - travelling back to player
            // Should travel through tiles
            Projectile.tileCollide = false;

            // Check if projectile is too far away and should just be killed
            float maxRange = 3000f;
            if (Vector2.DistanceSquared(Owner.Center, Projectile.Center) > maxRange * maxRange) {
                Projectile.Kill();
            }

            // Add to our velocity 
            float maxVelocity = ReturnSpeed * Owner.GetAttackSpeed(DamageClass.Melee);
            float homingStrength = 4f;
            Vector2 directionToPlayer = Owner.Center - Projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= homingStrength;
            Projectile.velocity += directionToPlayer;
            if (Projectile.velocity.LengthSquared() > maxVelocity * maxVelocity) {
                Projectile.velocity.Normalize();
                Projectile.velocity *= maxVelocity;
            }

            // Catch our projectile
            if (Main.myPlayer == Projectile.owner) {
                if (Projectile.getRect().Intersects(Main.player[Projectile.owner].getRect())) {
                    Projectile.Kill();
                }
            }
        }
    }
}