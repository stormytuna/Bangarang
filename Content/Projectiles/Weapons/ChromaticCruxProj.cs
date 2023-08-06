using System.IO;
using System.Linq;
using Bangarang.Common.Configs;
using Bangarang.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class ChromaticCruxProj : Boomerang
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Projectile.width = 30;
        Projectile.height = 30;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;

        Projectile.tileCollide = true;

        ReturnSpeed = 20f;
        HomingOnOwnerStrength = 3f;
        TravelOutFrames = 40;
        Rotation = 0.2f;
        DoTurn = true;
    }

    public override string Texture => "Bangarang/Content/Items/Weapons/ChromaticCrux";

    private int child = -1;

    public override void AI() {
        // Dust
        if (Main.rand.NextBool(2)) {
            Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f);
            int dust = Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.RainbowMk2, 0f, 0f, 0, color);
            Dust obj = Main.dust[dust];
            obj.velocity *= Main.rand.NextFromList(-1f, 1f) * 0.8f;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale = 0.9f + (Main.rand.NextFloat() * 0.9f);
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

    public override void OnSpawn(IEntitySource source) => child = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChromaticCruxRainbowProj>(), Projectile.damage / 5, Projectile.knockBack / 5f, Projectile.owner, 0f, Projectile.identity);

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        // Visuals - just copies the rainbow rod because I am uninspired :)
        ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.RainbowRodHit, new ParticleOrchestraSettings {
            PositionInWorld = target.Center,
            MovementVector = Projectile.velocity
        });

        // 'drop off' our rainbow effect
        Projectile rainbow = Main.projectile[child];
        if (rainbow.ai[0] == 0f) {
            rainbow.ai[0] = 1f;
            rainbow.ai[1] = target.whoAmI;
            rainbow.timeLeft = 2 * 60;
            rainbow.velocity = Projectile.velocity;
            rainbow.netUpdate = true;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

        if (Projectile.velocity.X != oldVelocity.X) {
            Projectile.velocity.X = 0f - oldVelocity.X;
        }

        if (Projectile.velocity.Y != oldVelocity.Y) {
            Projectile.velocity.Y = 0f - oldVelocity.Y;
        }

        return false;
    }

    public override void Kill(int timeLeft) {
        if (Main.projectile[child].ai[0] == 0f) {
            Main.projectile[child].Kill();
        }
    }

    public override void SendExtraAI(BinaryWriter writer) => writer.Write(child);

    public override void ReceiveExtraAI(BinaryReader reader) => child = reader.ReadInt32();

    private Asset<Texture2D> _glowMask;
    private Asset<Texture2D> GlowMask => _glowMask ??= ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/ChromaticCruxProj_GlowMask");

    public override bool PreDraw(ref Color lightColor) {
        Main.instance.LoadProjectile(Type);
        Asset<Texture2D> texture = TextureAssets.Projectile[Type];

        // Set some values we use
        Vector2 drawPos = Projectile.Center - Main.screenPosition;
        SpriteEffects effects = SpriteEffects.None;
        if (Projectile.spriteDirection == -1) {
            effects = SpriteEffects.FlipHorizontally;
        }

        Rectangle sourceRect = new(0, 0, texture.Width(), texture.Height());
        Vector2 origin = texture.Size() / 2f;
        Color drawColor = Projectile.GetAlpha(lightColor);

        // Draw our boomerang
        Main.EntitySpriteDraw(texture.Value, drawPos, sourceRect, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

        // Draw our glowmask
        drawColor = Color.White;
        Main.EntitySpriteDraw(GlowMask.Value, drawPos, sourceRect, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

        return false;
    }
}

public class ChromaticCruxRainbowProj : ModProjectile
{
    public override void SetStaticDefaults() =>
        ProjectileID.Sets.CultistIsResistantTo[Type] = true;

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

    private Projectile _parent;
    private Projectile Parent => _parent ??= Main.projectile.FirstOrDefault(proj => proj.identity == (int)Projectile.ai[1], Main.projectile.Last());
    private NPC EnemyToFollow => Main.npc[(int)Projectile.ai[1]];
    private ref float AI_Mode => ref Projectile.ai[0];

    public override void AI() {
        // Dust
        Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f);
        int dust = Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.RainbowMk2, 0f, 0f, 0, color);
        Dust obj = Main.dust[dust];
        obj.velocity *= Main.rand.NextFromList(-1f, 1f) * 0.8f;
        Main.dust[dust].noGravity = true;
        Main.dust[dust].scale = 0.9f + (Main.rand.NextFloat() * 0.9f);
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
            Projectile.rotation -= MathHelper.Lerp(0.01f, 0.2f, Projectile.timeLeft / 180f);

            // Check our enemy is still alive, if not find a new one
            if (!EnemyToFollow.active) {
                if (NPCHelpers.TryGetClosestEnemy(Projectile.Center, 20f * 16f, out NPC closestEnemy)) {
                    Projectile.ai[1] = closestEnemy.whoAmI;
                } else {
                    Projectile.velocity /= 1.1f;
                    return;
                }
            }

            GeneralHelpers.SmoothHoming(Projectile, EnemyToFollow.Center, 0.5f, 11f);
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        // Visuals - just copies the rainbow rod because I am uninspired :)
        ParticleOrchestraSettings settings = new() {
            PositionInWorld = target.Center,
            MovementVector = Projectile.velocity
        };
        ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.RainbowRodHit, settings);
    }

    public override void Kill(int timeLeft) {
        for (int i = 0; i < 20; i++) {
            Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f);
            int dust = Dust.NewDust(Projectile.position, Projectile.height, Projectile.width, DustID.RainbowMk2, 0f, 0f, 0, color);
            Dust obj = Main.dust[dust];
            obj.velocity *= Main.rand.NextFromList(-1f, 1f) * 0.8f;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale = 0.9f + (Main.rand.NextFloat() * 0.9f);
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

    private new Asset<Texture2D> Texture {
        get {
            _texture ??= ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/ChromaticCruxRainbowProj");

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