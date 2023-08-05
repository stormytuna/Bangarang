using Bangarang.Common.Configs;
using Bangarang.Common.GlobalProjectiles;
using Bangarang.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class YinAndRangProj : Boomerang
{
    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
        ProjectileID.Sets.TrailingMode[Type] = 5;
        SimpleTrailGlobalProjectile.ProjectileTrailSettings[Type] = new SimpleTrailSettings {
            StripColorFunction = GetStripColor,
            StripHalfWidthFunction = _ => 16f
        };
    }

    private Color GetStripColor(float progress) {
        float inverse = 1f - progress;
        Color color = Color.White * (inverse * inverse * inverse * inverse) * 0.3f;
        color.A = 0;
        return color;
    }

    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Projectile.width = 28;
        Projectile.height = 28;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;

        Projectile.tileCollide = true;

        ReturnSpeed = 18f;
        HomingOnOwnerStrength = 1.5f;
        TravelOutFrames = 30;
        DoTurn = true;
    }

    public override string Texture => "Bangarang/Content/Items/Weapons/YinAndRang";

    private bool HasSplit {
        get => Projectile.ai[2] == 1f;
        set => Projectile.ai[2] = value ? 1f : 0f;
    }

    public override void OnReachedApex() => Split();

    public override bool OnTileCollide(Vector2 oldVelocity) {
        Split();
        return base.OnTileCollide(oldVelocity);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        Split();
        base.OnHitNPC(target, hit, damageDone);
    }

    private void Split() {
        if (HasSplit || Main.myPlayer != Projectile.owner) {
            return;
        }

        // Spawn our projectiles
        Vector2 velocity = Projectile.rotation.ToRotationVector2();
        velocity.Normalize();
        velocity *= 5f;
        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<YinAndRangShardProj>(), Projectile.damage / 2, Projectile.knockBack / 2f, Projectile.owner, 0f, -1f);
        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -velocity, ModContent.ProjectileType<YinAndRangShardProj>(), Projectile.damage / 2, Projectile.knockBack / 2f, Projectile.owner, 1f, -1f);

        HasSplit = true;
    }
}

public class YinAndRangShardProj : ModProjectile
{
    public override void SetStaticDefaults() {
        Main.projFrames[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 30;
        ProjectileID.Sets.TrailingMode[Type] = 5;
        SimpleTrailGlobalProjectile.ProjectileTrailSettings[Type] = new SimpleTrailSettings {
            StripColorFunction = GetStripColor,
            StripHalfWidthFunction = _ => 4f
        };
    }

    private Color GetStripColor(float progress) {
        float inverse = 1f - progress;
        Color color = Color.White * (inverse * inverse * inverse * inverse) * 0.3f;
        //color.A = 0;
        return color;
    }

    public override void SetDefaults() {
        Projectile.width = 14;
        Projectile.height = 16;
        Projectile.aiStyle = -1;
        Projectile.extraUpdates = 2;
        Projectile.timeLeft = 180;

        Projectile.DamageType = DamageClass.Melee;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = 1;

        Projectile.tileCollide = true;
    }

    private ref float AI_State => ref Projectile.ai[0];

    private ref float AI_Target => ref Projectile.ai[1];

    private Dust MakeDust() {
        Color color = AI_State == 0f ? Color.Black : Color.White;
        return Dust.NewDustPerfect(Projectile.Center, DustID.Snow, Main.rand.NextVector2Square(0f, 1f), 40, color, Main.rand.NextFloat(0.4f, 0.8f));
    }

    public override void AI() {
        // Change our frame
        if (AI_State != 0f) {
            Projectile.frame = 1;
        }

        // Create dust
        if (Main.rand.NextBool(2)) {
            Dust dust = MakeDust();
            if (!Main.rand.NextBool(3)) {
                dust.velocity *= 2f;
                dust.noGravity = true;
                dust.scale *= 1.75f;
            } else {
                dust.scale *= 0.5f;
            }
        }

        // Point where its travelling
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        // Homing
        // Two states, either found target or not found target
        // Found target should use curved velocity homing towards it 
        if (AI_Target != -1f) {
            // Check our target is still alive
            NPC target = Main.npc[(int)AI_Target];
            if (!target.active) {
                AI_Target = -1f;
                Projectile.netUpdate = true;
                return;
            }

            GeneralHelpers.SmoothHoming(Projectile, target.Center, 0.2f, 5f, bufferZone: false);

            return;
        }

        // Not found target should just turn velocity to the left slightly
        float rotation = MathHelper.ToRadians(1f);
        Projectile.velocity = Projectile.velocity.RotatedBy(rotation);
        // Check for target
        if (NPCHelpers.TryGetClosestEnemy(Projectile.Center, 20f * 16f, out NPC closestEnemy)) {
            AI_Target = closestEnemy.whoAmI;
            Projectile.timeLeft += 20 * 60;
            Projectile.netUpdate = true;
        }
    }

    public override void Kill(int timeLeft) {
        // Dust explosion
        for (int i = 0; i < 13; i++) {
            Dust dust = MakeDust();
            if (!Main.rand.NextBool(3)) {
                dust.velocity *= 5f;
                dust.noGravity = true;
                dust.scale *= 1.75f;
            } else {
                dust.scale *= 0.5f;
            }
        }
    }
}