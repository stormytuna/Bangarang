using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles;

public class SpectralGlaiveProj : ModProjectile
{
    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

    public override void SetDefaults() {
        Projectile.width = 30;
        Projectile.height = 30;
        Projectile.aiStyle = -1;
        Projectile.alpha = 130;

        Projectile.DamageType = DamageClass.Generic;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 15;

        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
    }

    private Projectile Parent => Main.projectile[(int)Projectile.ai[0]];

    private ref float OrbitalRotation => ref Projectile.ai[1];

    public override void AI() {
        // Spin around our parent
        OrbitalRotation += 0.2f;
        // Spin the projectile itself
        Projectile.rotation += 0.3f;

        // Set our position 
        float distance = 3f * 16f;
        Vector2 offset = Vector2.UnitX * distance;
        offset = offset.RotatedBy(OrbitalRotation);
        Projectile.Center = Parent.Center + offset;

        // Set our velocity just in case
        Projectile.velocity = Vector2.Zero;

        // Make some funky dust and light
        if (Main.rand.NextBool(2)) {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff);
        }

        Lighting.AddLight(Projectile.Center, TorchID.UltraBright);

        // Check if we should kill our glaives
        if (!Parent.active) {
            Projectile.Kill();
        }
    }

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public override bool PreDraw(ref Color lightColor) {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++) {
            Main.EntitySpriteDraw(
                texture,
                Projectile.oldPos[i] + new Vector2(Projectile.width / 2, Projectile.height / 2) - Main.screenPosition,
                null,
                Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length),
                Projectile.rotation,
                texture.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0);
        }

        return false;
    }
}