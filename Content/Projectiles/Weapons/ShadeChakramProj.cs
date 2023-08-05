using Bangarang.Common.Configs;
using Bangarang.Common.GlobalProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
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
        SimpleTrailGlobalProjectile.ProjectileTrailSettings[Type] = new SimpleTrailSettings {
            StripColorFunction = GetStripColor,
            StripHalfWidthFunction = _ => 14f
        };
    }

    public override string Texture => "Bangarang/Content/Items/Weapons/ShadeChakram";

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

    private Color GetStripColor(float progress) {
        float inverse = 1f - progress;
        Color color = Color.Purple * (inverse * inverse * inverse * inverse) * 0.7f;
        color.A = 0;
        return color;
    }
}