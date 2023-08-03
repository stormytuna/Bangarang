using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class BonerangProj : Boomerang
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Projectile.width = 24;
        Projectile.height = 36;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;

        Projectile.tileCollide = true;

        ReturnSpeed = 16f;
        HomingOnOwnerStrength = 1.5f;
        TravelOutFrames = 30;
        DoTurn = true;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        int numShards = Main.rand.Next(1, 4);
        for (int i = 0; i < numShards; i++) {
            Vector2 velocity = Projectile.velocity;
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(30f));
            Projectile shard = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<BoneShardProj>(), Projectile.damage / 2, Projectile.knockBack / 3f, Projectile.owner, target.whoAmI);
            shard.frame = Main.rand.Next(0, 4);
        }

        // Dust
        int numDust = Main.rand.Next(4, 7);
        for (int i = 0; i < numDust; i++) {
            Vector2 velocity = Projectile.velocity;
            velocity *= 0.3f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone, velocity.X, velocity.Y, 0, default, Main.rand.NextFloat(0.8f, 1.2f));
        }

        Projectile.Kill();
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

        return true;
    }
}

public class BoneShardProj : ModProjectile
{
    public override void SetStaticDefaults() =>
        // DisplayName.SetDefault("Bone Shard");
        Main.projFrames[Type] = 4;

    public override void SetDefaults() {
        Projectile.width = 10;
        Projectile.height = 10;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 10;

        Projectile.tileCollide = true;
    }

    private ref float AI_IgnoreNPC => ref Projectile.ai[0];

    public override void AI() =>
        // Face where it's travelling 
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

    public override bool? CanHitNPC(NPC target) => target.whoAmI != (int)AI_IgnoreNPC && target.CanBeChasedBy();

    public override bool OnTileCollide(Vector2 oldVelocity) => true;

    public override void Kill(int timeLeft) {
        // Make some dust
        int numDust = Main.rand.Next(2, 6);
        for (int i = 0; i < numDust; i++) {
            Vector2 velocity = Projectile.velocity;
            velocity *= 0.3f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone, velocity.X, velocity.Y, 0, default, Main.rand.NextFloat(0.6f, 1f));
        }
    }
}