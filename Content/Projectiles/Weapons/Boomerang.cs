using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public abstract class Boomerang : ModProjectile {
        /// <summary>How quickly the boomerang will return to its owner. Default = 10f</summary>
        public float ReturnSpeed { get; set; } = 10f;

        /// <summary>How strong the boomerang will home in on its owner when returning. Default = 4f</summary>
        public float HomingOnOwnerStrength { get; set; } = 4f;

        /// <summary>How many frames the boomerang will travel away from the player for. Default = 30</summary>
        public int TravelOutFrames { get; set; } = 30;

        /// <summary>How many radians the boomerang will rotate per frame. Default = 0.4f</summary>
        public float Rotation { get; set; } = 0.4f;

        /// <summary>Whether or not the boomerang will turn around when it reaches its max TravelOutFrames. Default = true</summary>
        public bool DoTurn { get; set; } = true;

        public Player Owner { get => Main.player[Projectile.owner]; }

        public virtual void OnReachedApex() {

        }

        public override void AI() {
            // Funky sound
            if (Projectile.soundDelay == 0) {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            // Spinny :D
            Projectile.rotation += Rotation * Projectile.direction;

            // AI state 1 - travelling away from player 
            if (Projectile.ai[0] == 0f) {
                // Increase our frame counter
                Projectile.ai[1] += 1f;
                // Check if our frame counter is high enough to turn around
                if (Projectile.ai[1] >= (float)TravelOutFrames && DoTurn) {
                    OnReachedApex();
                    Projectile.ai[0] = 1f;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Projectile.ai[0] = 1f;
            Projectile.ai[1] = 0f;
            Projectile.netUpdate = true;
            Projectile.velocity = -Projectile.velocity;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = 20;
            height = 20;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.ai[0] = 1f;
            Projectile.ai[1] = 0f;
            Projectile.netUpdate = true;
            Projectile.velocity = -Projectile.velocity;

            return false;
        }
    }
}