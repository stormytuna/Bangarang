using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public class YinAndRangProj : Boomerang {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Yin and Rang");
            Main.projFrames[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 3;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults() {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.aiStyle = 3;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 16f;
            HomingOnOwnerStrength = 1.5f;
            TravelOutFrames = 30;
            DoTurn = true;
        }

        public override void OnReachedApex() {
            Split();
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Split();

            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Split();

            base.OnHitNPC(target, damage, knockback, crit);
        }

        private void Split() {
            // Change to our next frame
            Projectile.frame++;

            // Spawn our projectiles
            Vector2 velocity = Projectile.rotation.ToRotationVector2();
            velocity.Normalize();
            velocity *= 5f;
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<YinAndRangShardProj>(), Projectile.damage / 2, Projectile.knockBack / 2f, Projectile.owner, 0f, -1f);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -velocity, ModContent.ProjectileType<YinAndRangShardProj>(), Projectile.damage / 2, Projectile.knockBack / 2f, Projectile.owner, 1f, -1f);
        }

        private Asset<Texture2D> _yinAndRang;
        private Asset<Texture2D> YinAndRangTexture {
            get {
                if (_yinAndRang == null) {
                    _yinAndRang = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/YinAndRangProj");
                }
                return _yinAndRang;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            for (int i = 0; i < Projectile.oldPos.Length; i++) {
                int frameHeight = YinAndRangTexture.Value.Height / Main.projFrames[Type];
                int startY = frameHeight * Projectile.frame;
                Rectangle sourceRect = new(0, startY, YinAndRangTexture.Value.Width, frameHeight);

                Main.EntitySpriteDraw(
                    YinAndRangTexture.Value,
                    Projectile.oldPos[i] - Main.screenPosition + new Vector2(15f, 15f),
                    sourceRect,
                    Color.White * ((float)i / (float)Projectile.oldPos.Length) * 0.3f,
                    Projectile.rotation,
                    sourceRect.Size() / 2f,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                    );
            }

            return true;
        }
    }

    public class YinAndRangShardProj : ModProjectile {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Yin and Rang Shard");
            Main.projFrames[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 3;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults() {
            Projectile.width = 38;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 60;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;

            Projectile.tileCollide = true;
        }

        private ref float AI_State => ref Projectile.ai[0];
        private ref float AI_Target => ref Projectile.ai[1];

        private Dust MakeDust() {
            Color color = AI_State == 0f ? Color.Black : Color.White;
            return Dust.NewDustPerfect(Projectile.Center, DustID.Snow, Vector2.Zero, 40, color, Main.rand.NextFloat(0.4f, 0.8f));
        }

        public override void AI() {
            // Change our frame
            if (AI_State != 0f) {
                Projectile.frame = 1;
            }

            // Create dust
            if (Main.rand.NextBool(2)) {
                Dust dust = MakeDust();
                if (!Main.rand.NextBool(3)) {
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    dust.scale *= 1.75f;
                }
                else {
                    dust.scale *= 0.5f;
                }
            }

            // Point where its travelling
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            // Homing
            // Two states, either found target or not found target
            // Found target should use curved velocity homing towards it 
            if (AI_Target != -1f) {
                // Check our target is still alive
                var target = Main.npc[(int)AI_Target];
                if (!target.active) {
                    AI_Target = -1f;
                    Projectile.netUpdate = true;
                    return;
                }

                float rotationMax = MathHelper.ToRadians(8f);
                float rotTarget = Utils.ToRotation(target.Center - Projectile.Center);
                float rotCurrent = Utils.ToRotation(Projectile.velocity);
                Projectile.velocity = Utils.RotatedBy(Projectile.velocity, MathHelper.WrapAngle(MathHelper.WrapAngle(Utils.AngleTowards(rotCurrent, rotTarget, rotationMax)) - Utils.ToRotation(Projectile.velocity)));

                return;
            }

            // Not found target should just turn velocity to the left slightly
            float rotation = MathHelper.ToRadians(1f);
            Projectile.velocity = Projectile.velocity.RotatedBy(rotation);
            // Check for target
            var npc = Helpers.GetClosestEnemy(Projectile.Center, 20f * 16f, true, true);
            if (npc is not null) {
                AI_Target = npc.whoAmI;
                Projectile.timeLeft += 10 * 60;
                Projectile.netUpdate = true;
            }
        }

        public override void Kill(int timeLeft) {
            // Dust explosion
            for (int i = 0; i < 13; i++) {
                Dust dust = MakeDust();
                if (!Main.rand.NextBool(3)) {
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    dust.scale *= 1.75f;
                }
                else {
                    dust.scale *= 0.5f;
                }
            }
        }

        private Asset<Texture2D> _yinAndRangShard;
        private Asset<Texture2D> YinAndRangShardTexture {
            get {
                if (_yinAndRangShard == null) {
                    _yinAndRangShard = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/YinAndRangShardProj");
                }
                return _yinAndRangShard;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            for (int i = 0; i < Projectile.oldPos.Length; i++) {
                int frameHeight = YinAndRangShardTexture.Value.Height / Main.projFrames[Type];
                int startY = frameHeight * Projectile.frame;
                Rectangle sourceRect = new(0, startY, YinAndRangShardTexture.Value.Width, frameHeight);

                Main.EntitySpriteDraw(
                    YinAndRangShardTexture.Value,
                    Projectile.oldPos[i] - Main.screenPosition + new Vector2(15f, 15f),
                    sourceRect,
                    Color.White * ((float)i / (float)Projectile.oldPos.Length) * 0.3f,
                    Projectile.rotation,
                    sourceRect.Size() / 2f,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                    );
            }

            return true;
        }
    }
}