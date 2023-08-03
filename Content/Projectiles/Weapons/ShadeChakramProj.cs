using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class ShadeChakramProj : Boomerang
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
        ProjectileID.Sets.TrailingMode[Type] = 5;
        ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;
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

        ReturnSpeed = 24f;
        HomingOnOwnerStrength = 2f;
        TravelOutFrames = 25;
        DoTurn = true;
    }

    private static readonly VertexStrip vertexStrip = new();

    public override bool PreDraw(ref Color lightColor) {
        MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
        miscShaderData.UseSaturation(-2.8f);
        miscShaderData.UseOpacity(2f);
        miscShaderData.Apply();

        vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, GetStripColor, _ => 14f, (Projectile.Size / 2f) - Main.screenPosition);
        vertexStrip.DrawTrail();

        Main.pixelShader.CurrentTechnique.Passes[0].Apply();

        return true;
    }

    private Color GetStripColor(float progress) {
        float inverse = 1f - progress;
        Color color = Color.Purple * (inverse * inverse * inverse * inverse) * 0.7f;
        color.A = 0;
        return color;
    }
}