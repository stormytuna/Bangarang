using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public class WhiteDwarfProj : Boomerang {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("White Dwarf");
        }

        public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

        public override void SetDefaults() {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = 3;
            Projectile.extraUpdates = 1;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;

            ReturnSpeed = 21f;
            HomingOnOwnerStrength = 1.2f;
            TravelOutFrames = 25;
            Rotation = 0f;
            DoTurn = true;
        }

        public override void AI() {
            // If an enemy is nearby, apply daybroken
            var nearbyNPCs = Helpers.GetNearbyEnemies(Projectile.Center, 3f * 16f, true, false);
            foreach (var npc in nearbyNPCs) {
                npc.AddBuff(BuffID.Daybreak, 60);
            }

            // Dust trail
            for (int i = 0; i < 2; i++) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
            }

            // Point where we're travelling
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Explode();

            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            Explode();

            base.OnHitNPC(target, damage, knockback, crit);
        }

        private void Explode() {
            // Explosion
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileID.SolarWhipSwordExplosion, Projectile.damage, 10f, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);

            // Dust
            int numDust = Main.rand.Next(4, 7);
            for (int i = 0; i < numDust; i++) {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                if (Main.rand.NextBool(3)) {
                    dust.noGravity = true;
                    dust.velocity *= 8f;
                    dust.scale = Main.rand.NextFloat(1.2f, 1.5f);
                }
                else {
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                    dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
                }
            }
        }

        private Asset<Texture2D> _texture;
        private Asset<Texture2D> _effect;
        new private Asset<Texture2D> Texture {
            get {
                if (_texture == null) {
                    _texture = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/WhiteDwarfProj");
                }
                return _texture;
            }
        }
        private Asset<Texture2D> Effect {
            get {
                if (_effect == null) {
                    _effect = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/WhiteDwarfEffect");
                }
                return _effect;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.Camera.Sampler, DepthStencilState.None, Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.Draw(
                Effect.Value,
                Projectile.Center - Main.screenPosition + Projectile.velocity * -0.8f,
                new Rectangle(0, 0, Effect.Width(), Effect.Height()),
                GetColor(),
                Projectile.rotation,
                new Rectangle(0, 0, Effect.Width(), Effect.Height()).Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.Camera.Sampler, DepthStencilState.None, Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.Draw(
                Texture.Value,
                Projectile.Center - Main.screenPosition,
                new Rectangle(0, 0, Texture.Width(), Texture.Height()),
                Color.White,
                Projectile.rotation,
                new Rectangle(0, 0, Texture.Width(), Texture.Height()).Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0);

            return false;
        }

        private Color GetColor() {
            float sineTime = MathF.Sin(Main.LocalPlayer.miscCounterNormalized);
            Color color = Color.Lerp(Color.Orange, Color.OrangeRed, sineTime);
            return color;
        }
    }
}