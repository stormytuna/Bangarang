using System.IO;
using Bangarang.Common.Configs;
using Bangarang.Common.GlobalProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class WhiteDwarfProj : Boomerang
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
        ProjectileID.Sets.TrailingMode[Type] = 5;
        SimpleTrailGlobalProjectile.ProjectileTrailSettings[Type] = new SimpleTrailSettings {
            StripColorFunction = GetStripColor,
            StripHalfWidthFunction = GetStripHalfWidth
        };
    }

    private Color GetStripColor(float progress) {
        float inverse = 1f - progress;
        Color color = Color.OrangeRed * (inverse * inverse * inverse * inverse) * 0.3f;
        color.A = 0;
        return color;
    }

    private float GetStripHalfWidth(float progress) {
        float inverse = 1f - progress;
        return inverse * 10f;
    }

    public override void SetDefaults() {
        Projectile.width = 50;
        Projectile.height = 50;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.tileCollide = true;

        ReturnSpeed = 21f;
        HomingOnOwnerStrength = 1.2f;
        TravelOutFrames = 25;
        Rotation = 0.4f;
        DoTurn = true;
    }

    public override string Texture => "Bangarang/Content/Items/Weapons/WhiteDwarf";

    public override void AI() {
        // Dust trail
        for (int i = 0; i < 2; i++) {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            d.noGravity = true;
        }

        base.AI();
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

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        if (Projectile.ai[0] == 0f) {
            Explode(target.Center);
        }

        target.AddBuff(BuffID.Daybreak, 5 * 60);
    }

    private void Explode(Vector2 position) {
        // Explosion
        if (Projectile.owner == Main.myPlayer) {
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), position, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, Projectile.damage, 10f, Projectile.owner, 0f, 0.85f + (Main.rand.NextFloat() * 1.15f));
        }

        // Dust
        int numDust = Main.rand.Next(4, 7);
        for (int i = 0; i < numDust; i++) {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
            if (Main.rand.NextBool(3)) {
                dust.noGravity = true;
                dust.velocity *= 8f;
                dust.scale = Main.rand.NextFloat(1.2f, 1.5f);
            } else {
                dust.noGravity = true;
                dust.velocity *= 2f;
                dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
            }
        }
    }

    public override void SendExtraAI(BinaryWriter writer) => writer.Write(Projectile.localAI[0]);

    public override void ReceiveExtraAI(BinaryReader reader) => Projectile.localAI[0] = reader.ReadSingle();
}