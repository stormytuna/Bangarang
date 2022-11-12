using Bangarang.Common.Configs;
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

        public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

        public override void SetDefaults() {
            Projectile.width = 32;
            Projectile.height = 30;
            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 15f;
            HomingOnOwnerStrength = 1.2f;
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
                    float homingStrength = 0.7f;
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