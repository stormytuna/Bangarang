using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs {
    public class IceBoomerangGI : GlobalItem {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.IceBoomerang && ServerConfig.Instance.VanillaChanges;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            int index = tooltips.FindLastIndex(t => t.Mod == "Terraria");
            tooltips.Insert(index + 1, new(Mod, "Tooltip0", "Sprays icey bolts and inflicts Frostburn when it hits an enemy"));
        }
    }

    public class IceBoomerangGP : GlobalProjectile {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.IceBoomerang && ServerConfig.Instance.VanillaChanges;

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) {
            for (int i = 0; i < 2; i++) {
                Projectile shard = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<IceBoomerangShard>(), projectile.damage / 2, projectile.knockBack / 3f, projectile.owner);
                Vector2 velocity = new(0f, -1f);
                velocity = velocity.RotatedByRandom(MathHelper.Pi / 4f);
                velocity *= Main.rand.NextFloat(2f, 4f);
                shard.velocity = velocity;
                shard.localNPCImmunity[target.whoAmI] = 10;
            }
        }
    }

    public class IceBoomerangShard : ModProjectile {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ice Shard");
        }

        public override void SetDefaults() {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 1;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.coldDamage = true;
        }

        public override void AI() {
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Frost, Projectile.velocity.X, Projectile.velocity.Y, 50, default, 1.2f);
            Main.dust[d].noGravity = true;
            Dust dust2 = Main.dust[d];
            dust2.velocity *= 0.3f;
        }

        public override void Kill(int timeLeft) {
            int numDust = 10;

            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            for (int i = 0; i < numDust; i++) {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Frost);
                if (!Main.rand.NextBool(3)) {
                    Dust dust = Main.dust[d];
                    dust.velocity *= 2f;
                    Main.dust[d].noGravity = true;
                    dust = Main.dust[d];
                    dust.scale *= 1.75f;
                }
                else {
                    Dust dust = Main.dust[d];
                    dust.scale *= 0.5f;
                }
            }
        }
    }
}