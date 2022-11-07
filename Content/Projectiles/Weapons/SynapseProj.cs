using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public class SynapseProj : Boomerang {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Synapse");
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults() {
            Projectile.width = 32;
            Projectile.height = 30;
            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 14f;
            HomingOnOwnerStrength = 4f;
            TravelOutFrames = 30;
            DoTurn = true;
        }

        private int currentTarget = -1;

        public override void AI() {
            // Only want to home in on enemies while travelling out
            if (Projectile.ai[0] == 0f) {
                var target = Helpers.GetClosestEnemy(Projectile.Center, 20f * 16f, true, true);

                // If there's an npc near the boomerang, we want to move towards it
                if (target != null) {
                    DoTurn = false;
                    // Add to our velocity 
                    float maxVelocity = ReturnSpeed * Owner.GetAttackSpeed(DamageClass.Melee);
                    float homingStrength = 1.5f;
                    Vector2 toEnemy = target.Center - Projectile.Center;
                    toEnemy.Normalize();
                    toEnemy *= homingStrength;
                    Projectile.velocity += toEnemy;
                    if (Projectile.velocity.LengthSquared() > maxVelocity * maxVelocity) {
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= maxVelocity;
                    }
                }

                DoTurn = true;
            }

            // Calling base so we have the default AI provided by our Boomerang class
            base.AI();
        }

        public override void SendExtraAI(BinaryWriter writer) {
            writer.Write(currentTarget);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            currentTarget = reader.ReadInt32();
        }
    }
}