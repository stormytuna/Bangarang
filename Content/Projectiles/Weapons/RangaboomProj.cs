using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public class RangaboomProj : Boomerang {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Rangaboom");
        }

        public override void SetDefaults() {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 21f;
            HomingOnOwnerStrength = 1.5f;
            TravelOutFrames = 30;
            DoTurn = true;
        }

        private ref float AI_Mode => ref Projectile.ai[0];
        private ref float AI_SpawnPortal => ref Projectile.ai[1];

        private Projectile SpawnPortal => Main.projectile[(int)AI_SpawnPortal];

        private bool TileCheck() {
            Point start = Projectile.position.ToTileCoordinates();
            Point end = (Projectile.position + new Vector2(Projectile.width, Projectile.height)).ToTileCoordinates();
            return WorldGen.EmptyTileCheck(start.X, end.X, start.Y, end.Y);
        }

        public override void OnSpawn(IEntitySource source) {
            AI_SpawnPortal = (int)Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RangaboomPortalProj>(), 0, 0f, Projectile.owner, Projectile.whoAmI);

            if (!TileCheck()) {
                Projectile.tileCollide = false;
            }
        }

        public override void AI() {
            // Not calling base here since this boomerang acts very differently to our others 
            // General tasks
            // Funky sound
            if (Projectile.soundDelay == 0) {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            // Spinny :D
            Projectile.rotation += Rotation * Projectile.direction;

            // Mode 0 => travelling from portal to player
            if (AI_Mode == 0f) {
                // Since we can spawn in tiles like starfury stars, check that we are no longer in tiles
                if (!Projectile.tileCollide && TileCheck()) {
                    Projectile.tileCollide = true;
                }

                // See if our projectile is too far away
                float xDif = Owner.Center.X - Projectile.Center.X;
                float yDif = Owner.Center.Y - Projectile.Center.Y;
                float dist = MathF.Sqrt(xDif * xDif + yDif * yDif);
                if (dist > 3000f) {
                    Projectile.Kill();
                }

                // Get our x and y velocity values
                float mult = ReturnSpeed / dist;
                float xVel = xDif * mult;
                float yVel = yDif * mult;

                // Increase or decrease our X velocity accordingly 
                if (Projectile.velocity.X < xVel) {
                    Projectile.velocity.X += HomingOnOwnerStrength;
                    if (Projectile.velocity.X < 0f && xVel > 0f) {
                        Projectile.velocity.X += HomingOnOwnerStrength;
                    }
                }
                else if (Projectile.velocity.X > xVel) {
                    Projectile.velocity.X -= HomingOnOwnerStrength;
                    if (Projectile.velocity.X > 0f && xVel < 0f) {
                        Projectile.velocity.X -= HomingOnOwnerStrength;
                    }
                }

                // Same for our Y velocity
                if (Projectile.velocity.Y < yVel) {
                    Projectile.velocity.Y += HomingOnOwnerStrength;
                    if (Projectile.velocity.Y < 0f && yVel > 0f) {
                        Projectile.velocity.Y += HomingOnOwnerStrength;
                    }
                }
                else if (Projectile.velocity.Y > yVel) {
                    Projectile.velocity.Y -= HomingOnOwnerStrength;
                    if (Projectile.velocity.Y > 0f && yVel < 0f) {
                        Projectile.velocity.Y -= HomingOnOwnerStrength;
                    }
                }

                // "Catch" our projectile
                if (Main.myPlayer == Projectile.owner) {
                    if (Projectile.getRect().Intersects(Main.player[Projectile.owner].getRect())) {
                        Projectile.ai[0] = 1f;
                        Projectile.velocity = -Projectile.velocity;
                        Projectile.netUpdate = true;
                    }
                }
            }

            // No guard clause because funy scope issues
            else {
                // Mode 1 => travelling to portal
                Projectile.tileCollide = false;

                // See if our projectile is too far away
                float xDif = SpawnPortal.Center.X - Projectile.Center.X;
                float yDif = SpawnPortal.Center.Y - Projectile.Center.Y;
                float dist = MathF.Sqrt(xDif * xDif + yDif * yDif);
                if (dist > 3000f) {
                    Projectile.Kill();
                }

                // Get our x and y velocity values
                float mult = ReturnSpeed / dist;
                float xVel = xDif * mult;
                float yVel = yDif * mult;

                // Increase or decrease our X velocity accordingly 
                if (Projectile.velocity.X < xVel) {
                    Projectile.velocity.X += HomingOnOwnerStrength;
                    if (Projectile.velocity.X < 0f && xVel > 0f) {
                        Projectile.velocity.X += HomingOnOwnerStrength;
                    }
                }
                else if (Projectile.velocity.X > xVel) {
                    Projectile.velocity.X -= HomingOnOwnerStrength;
                    if (Projectile.velocity.X > 0f && xVel < 0f) {
                        Projectile.velocity.X -= HomingOnOwnerStrength;
                    }
                }

                // Same for our Y velocity
                if (Projectile.velocity.Y < yVel) {
                    Projectile.velocity.Y += HomingOnOwnerStrength;
                    if (Projectile.velocity.Y < 0f && yVel > 0f) {
                        Projectile.velocity.Y += HomingOnOwnerStrength;
                    }
                }
                else if (Projectile.velocity.Y > yVel) {
                    Projectile.velocity.Y -= HomingOnOwnerStrength;
                    if (Projectile.velocity.Y > 0f && yVel < 0f) {
                        Projectile.velocity.Y -= HomingOnOwnerStrength;
                    }
                }

                // "Catch" our projectile
                if (dist <= 50f) {
                    Projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft) {
            SpawnPortal.Kill();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if (Projectile.ai[0] == 0f) {
                Projectile.ai[0] = 1f;
                Projectile.velocity = -Projectile.velocity;
                Projectile.netUpdate = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            if (Projectile.ai[0] == 0f) {
                Projectile.ai[0] = 1f;
                Projectile.velocity = -Projectile.velocity;
                Projectile.netUpdate = true;
            }

            return false;
        }

        private Asset<Texture2D> _glowMask;
        private Asset<Texture2D> GlowMask {
            get {
                if (_glowMask is null) {
                    _glowMask = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/RangaboomProj_GlowMask");
                }
                return _glowMask;
            }
        }

        public override void PostDraw(Color lightColor) {
            Rectangle sourceRect = new(0, Projectile.frame * GlowMask.Height(), GlowMask.Width(), GlowMask.Height() / Main.projFrames[Type]);
            Main.EntitySpriteDraw(
                GlowMask.Value,
                Projectile.Center - Main.screenPosition,
                sourceRect,
                Color.White,
                Projectile.rotation,
                sourceRect.Size() / 2f,
                Projectile.scale,
                Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0);
        }
    }

    public class RangaboomPortalProj : ModProjectile {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Rangaboom Portal");
            Main.projFrames[Type] = 8;
        }

        public override void SetDefaults() {
            Projectile.width = 32;
            Projectile.height = 44;
            Projectile.aiStyle = -1;
        }

        public override void OnSpawn(IEntitySource source) {
            // Dust
            for (int i = 0; i < 15; i++) {
                Vector2 velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(4f, 10f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, 253, velocity, 120, default, Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
            }
        }

        private Projectile Parent => Main.projectile[(int)Projectile.ai[0]];

        public override void AI() {
            // Point at our boomerang
            Projectile.rotation = (Parent.Center - Projectile.Center).ToRotation();

            // Cycle through frames
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Type]) {
                    Projectile.frame = 0;
                }
            }

            // Dust "falling" into portal
            if (Main.rand.NextBool(2)) {
                Vector2 spawnPos = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi);
                Vector2 velocity = -spawnPos * 2f;
                Dust d = Dust.NewDustPerfect((spawnPos * 20f) + Projectile.Center, 253, velocity, 120, default, Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
            }
        }

        public override void Kill(int timeLeft) {
            // Dust
            for (int i = 0; i < 15; i++) {
                Vector2 velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(4f, 10f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, 253, velocity, 120, default, Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
            }
        }

        private Asset<Texture2D> _texture;
        private Asset<Texture2D> _wibble;
        new private Asset<Texture2D> Texture {
            get {
                if (_texture is null) {
                    _texture = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/RangaboomPortalProj");
                }
                return _texture;
            }
        }
        private Asset<Texture2D> Wibble {
            get {
                if (_wibble is null) {
                    _wibble = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/RangaboomPortalProj_Wibble");
                }
                return _wibble;
            }
        }


        public override bool PreDraw(ref Color lightColor) {
            // Draw our wibbly effect
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            int frameHeight = Wibble.Height() / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, Wibble.Width(), frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            SpriteEffects effects = SpriteEffects.None;
            if ((Parent.Center - Projectile.Center).X < 0) {
                effects = SpriteEffects.FlipVertically;
            }
            const float TwoPi = MathHelper.TwoPi;
            float shineScale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 1.1f;
            Color effectColor = lightColor;
            effectColor.A = 0;
            effectColor = effectColor * 0.1f * shineScale;
            for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI)) {
                Main.EntitySpriteDraw(Wibble.Value, drawPos + (TwoPi * num5).ToRotationVector2() * 4f, sourceRectangle, effectColor, Projectile.rotation, origin, Projectile.scale, effects, 0);
            }

            // Draw our original projectile
            Main.EntitySpriteDraw(Texture.Value, drawPos, sourceRectangle, Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0);

            return false;
        }
    }
}