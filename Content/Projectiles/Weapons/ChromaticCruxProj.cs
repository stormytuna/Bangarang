using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public class ChromaticCruxProj : Boomerang {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Chromatic Crux");
        }

        public override void SetDefaults() {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 18f;
            HomingOnOwnerStrength = 3f;
            TravelOutFrames = 40;
            Rotation = 0.2f;
            DoTurn = true;
        }

        int child = -1;

        public override void AI() {
            // Dust
            if (Main.rand.NextBool(2)) {
                Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f, byte.MaxValue);
                int dust = Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.RainbowMk2, 0f, 0f, 0, color, 1f);
                Dust obj = Main.dust[dust];
                obj.velocity *= Main.rand.NextFromList(-1f, 1f) * 0.8f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 0.9f + Main.rand.NextFloat() * 0.9f;
                Main.dust[dust].fadeIn = Main.rand.NextFloat() * 0.9f;
                if (Main.dust[dust].dustIndex != 6000) {
                    Dust obj2 = Dust.CloneDust(dust);
                    obj2.scale /= 2f;
                    obj2.fadeIn *= 0.85f;
                    obj2.color = new Color(255, 255, 255, 255);
                }
            }

            base.AI();
        }

        public override void OnSpawn(IEntitySource source) {
            child = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChromaticCruxRainbowProj>(), Projectile.damage / 5, Projectile.knockBack / 5f, Projectile.owner, 0f, Projectile.whoAmI);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            // Visuals - just copies the rainbow rod because I am uninspired :)
            var settings = new ParticleOrchestraSettings {
                PositionInWorld = target.Center,
                MovementVector = Projectile.velocity
            };
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.RainbowRodHit, settings);

            // 'drop off' our rainbow effect
            Projectile rainbow = Main.projectile[child];
            if (rainbow.ai[0] == 0f) {
                rainbow.ai[0] = 1f;
                rainbow.ai[1] = target.whoAmI;
                rainbow.timeLeft = 2 * 60;
                rainbow.velocity = Projectile.velocity;
                rainbow.netUpdate = true;
                Projectile.netUpdate = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = 0f - oldVelocity.X;

            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = 0f - oldVelocity.Y;

            return false;
        }

        public override void Kill(int timeLeft) {
            if (Main.projectile[child].ai[0] == 0f) {
                Main.projectile[child].Kill();
            }
        }

        public override void SendExtraAI(BinaryWriter writer) {
            writer.Write(child);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            child = reader.ReadInt32();
        }

        private Asset<Texture2D> _texture;
        private Asset<Texture2D> _glowMask;

        new private Asset<Texture2D> Texture {
            get {
                if (_texture is null) {
                    _texture = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/ChromaticCruxProj");
                }
                return _texture;
            }
        }
        private Asset<Texture2D> GlowMask {
            get {
                if (_glowMask is null) {
                    _glowMask = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/ChromaticCruxProj_GlowMask");
                }
                return _glowMask;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            // Set some values we use
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1) {
                effects = SpriteEffects.FlipHorizontally;
            }
            Rectangle sourceRect = new(0, 0, Texture.Width(), Texture.Height());
            Vector2 origin = Texture.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            // Draw our boomerang
            Main.EntitySpriteDraw(Texture.Value, drawPos, sourceRect, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            // Draw our glowmask
            drawColor = Color.White;
            Main.EntitySpriteDraw(GlowMask.Value, drawPos, sourceRect, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            return false;
        }
    }

    public class ChromaticCruxRainbowProj : ModProjectile {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Chromatic Rainbow");
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults() {
            Projectile.width = 102;
            Projectile.height = 102;
            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        private Projectile Parent => Main.projectile[(int)Projectile.ai[1]];
        private NPC EnemyToFollow => Main.npc[(int)Projectile.ai[1]];
        private ref float AI_Mode => ref Projectile.ai[0];

        public override void AI() {
            // Dust
            Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f, byte.MaxValue);
            int dust = Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.RainbowMk2, 0f, 0f, 0, color, 1f);
            Dust obj = Main.dust[dust];
            obj.velocity *= Main.rand.NextFromList(-1f, 1f) * 0.8f;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale = 0.9f + Main.rand.NextFloat() * 0.9f;
            Main.dust[dust].fadeIn = Main.rand.NextFloat() * 0.9f;
            if (Main.dust[dust].dustIndex != 6000) {
                Dust obj2 = Dust.CloneDust(dust);
                obj2.scale /= 2f;
                obj2.fadeIn *= 0.85f;
                obj2.color = new Color(255, 255, 255, 255);
            }

            // Mode 0 is sticking to the boomerang that created it
            if (AI_Mode == 0f) {
                Projectile.Center = Parent.Center;
                Projectile.rotation -= 0.2f;
            }
            // Mode 1 is homing in on the enemy that it's bound to
            else {
                // Lower our rotation to it spins down
                Projectile.rotation -= MathHelper.Lerp(0.01f, 0.2f, ((float)Projectile.timeLeft / 180f));

                // Check our enemy is still alive, if not find a new one
                if (!EnemyToFollow.active) {
                    var closestEnemy = Helpers.GetClosestEnemy(Projectile.Center, 20f * 16f, false, true);
                    if (closestEnemy is not null) {
                        Projectile.ai[1] = (float)closestEnemy.whoAmI;
                    }
                    else {
                        Projectile.velocity /= 1.1f;
                        return;
                    }
                }

                // Homing - copy pasted from Boomerang.cs B)
                // Get our x and y velocity values
                float xDif = EnemyToFollow.Center.X - Projectile.Center.X;
                float yDif = EnemyToFollow.Center.Y - Projectile.Center.Y;
                float dist = MathF.Sqrt(xDif * xDif + yDif * yDif);

                // If we're super close to the npc there's no point changing our velocity so just divide it to slow down and return
                if (dist <= 32f) {
                    Projectile.velocity /= 1.1f;
                    return;
                }

                // Get values for our max velocity
                float mult = 9f / dist;
                float xVel = xDif * mult;
                float yVel = yDif * mult;

                // Increase or decrease our X velocity accordingly 
                if (Projectile.velocity.X < xVel) {
                    Projectile.velocity.X += 0.9f;
                    if (Projectile.velocity.X < 0f && xVel > 0f) {
                        Projectile.velocity.X += 0.9f;
                    }
                }
                else if (Projectile.velocity.X > xVel) {
                    Projectile.velocity.X -= 0.9f;
                    if (Projectile.velocity.X > 0f && xVel < 0f) {
                        Projectile.velocity.X -= 0.9f;
                    }
                }

                // Same for our Y velocity
                if (Projectile.velocity.Y < yVel) {
                    Projectile.velocity.Y += 0.9f;
                    if (Projectile.velocity.Y < 0f && yVel > 0f) {
                        Projectile.velocity.Y += 0.9f;
                    }
                }
                else if (Projectile.velocity.Y > yVel) {
                    Projectile.velocity.Y -= 0.9f;
                    if (Projectile.velocity.Y > 0f && yVel < 0f) {
                        Projectile.velocity.Y -= 0.9f;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            // Visuals - just copies the rainbow rod because I am uninspired :)
            var settings = new ParticleOrchestraSettings {
                PositionInWorld = target.Center,
                MovementVector = Projectile.velocity
            };
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.RainbowRodHit, settings);
        }

        public override void Kill(int timeLeft) {
            for (int i = 0; i < 20; i++) {
                Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f, byte.MaxValue);
                int dust = Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.RainbowMk2, 0f, 0f, 0, color, 1f);
                Dust obj = Main.dust[dust];
                obj.velocity *= Main.rand.NextFromList(-1f, 1f) * 0.8f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 0.9f + Main.rand.NextFloat() * 0.9f;
                Main.dust[dust].fadeIn = Main.rand.NextFloat() * 0.9f;
                if (Main.dust[dust].dustIndex != 6000) {
                    Dust obj2 = Dust.CloneDust(dust);
                    obj2.scale /= 2f;
                    obj2.fadeIn *= 0.85f;
                    obj2.color = new Color(255, 255, 255, 255);
                }
            }
        }

        private Asset<Texture2D> _texture;
        new private Asset<Texture2D> Texture {
            get {
                if (_texture is null) {
                    _texture = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/ChromaticCruxRainbowProj");
                }
                return _texture;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Rectangle drawRect = new(0, 0, Texture.Width(), Texture.Height());
            Vector2 origin = Texture.Size() / 2f;
            Color drawColor = Projectile.GetFairyQueenWeaponsColor(0.25f, 0f, (0.33f + Main.GlobalTimeWrappedHourly) % 1f);
            Main.EntitySpriteDraw(Texture.Value, drawPos, drawRect, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}