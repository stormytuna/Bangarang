using Bangarang.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.GlobalProjectiles;

public class IlluminantCoatingGlobalProjectile : GlobalProjectile
{
    public override void PostAI(Projectile projectile) {
        BangarangPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<BangarangPlayer>();

        // Return faster
        if (modPlayer.BoomerangReturnSpeedMult > 0 && projectile.ai[0] == 1f) {
            projectile.position += projectile.velocity * modPlayer.BoomerangReturnSpeedMult;
        }

        // Glow and dust
        if (modPlayer.BoomerangGlowAndDust) {
            Lighting.AddLight(projectile.Center, TorchID.Purple);

            if (Main.rand.NextBool(10)) {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.UndergroundHallowedEnemies);
                dust.velocity *= 0.8f;
            }
        }
    }

    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
        BangarangPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<BangarangPlayer>();
        modifiers.Knockback *= modPlayer.BoomerangKnockbackMult;
    }
}