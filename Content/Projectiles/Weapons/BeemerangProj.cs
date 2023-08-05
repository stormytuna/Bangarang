using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class BeemerangProj : Boomerang
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Projectile.width = 36;
        Projectile.height = 36;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;

        Projectile.tileCollide = true;

        ReturnSpeed = 17f;
        HomingOnOwnerStrength = 1.2f;
        TravelOutFrames = 30;
        DoTurn = true;
    }

    public override string Texture => "Bangarang/Content/Items/Weapons/Beemerang";

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        int numBees = 1;
        if (Main.rand.NextBool(3)) {
            numBees++;
        }

        for (int i = 0; i < numBees; i++) {
            Vector2 velocity = new(Main.rand.NextFloat(0f, 0.3f), 0f);
            velocity = velocity.RotatedByRandom(MathHelper.TwoPi);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, Owner.beeType(), Owner.beeDamage(damageDone), Owner.beeKB(hit.Knockback), Projectile.owner);
        }
    }
}