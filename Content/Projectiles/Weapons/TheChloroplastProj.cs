using System.Collections.Generic;
using Bangarang.Common.Configs;
using Bangarang.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class TheChloroplastProj : Boomerang
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Projectile.width = 54;
        Projectile.height = 54;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.tileCollide = true;

        TravelOutFrames = 60;
        Rotation = 0.3f;
        DoTurn = true;
    }

    public override string Texture => "Bangarang/Content/Items/Weapons/TheChloroplast";

    public override void AI() {
        // Using ai[0] as our "stay in place" mode
        Projectile.velocity /= 1.05f;
        Rotation /= 1.02f;

        // Dust
        for (int i = 0; i < 2; i++) {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.ChlorophyteWeapon, 0f, 0f, 0, default, Main.rand.NextFloat(0.8f, 1.2f));
            d.noGravity = true;
        }

        base.AI();
    }

    public override void OnReachedApex() => Projectile.Kill();

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }

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

    public override void OnKill(int timeLeft) {
        // Smart fire at targets
        // Get our list of nearby enemies
        List<NPC> closeNPCs = NPCHelpers.GetNearbyEnemies(Projectile.Center, 30f * 16f);

        for (int i = 0; i < 3; i++) {
            bool isRandom = i >= closeNPCs.Count;
            Vector2 velocity = isRandom ? new Vector2(0f, 1f) : closeNPCs[i].Center - Projectile.Center;
            velocity.Normalize();
            if (isRandom) {
                velocity = velocity.RotatedByRandom(MathHelper.TwoPi);
            }

            velocity *= Main.rand.NextFloat(16f, 20f);
            Projectile stinger = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileID.Stinger, Projectile.damage / 2, Projectile.knockBack / 3f, Projectile.owner);
            stinger.DamageType = DamageClass.Melee;
            stinger.penetrate = 1;
            stinger.hostile = false;
            stinger.friendly = true;
            stinger.timeLeft = 5 * 60;
        }

        // Effects
        for (int i = 0; i < 24; i++) {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.ChlorophyteWeapon, 0f, 0f, 0, default, Main.rand.NextFloat(1.4f, 1.8f));
            d.noGravity = true;
            d.velocity *= 6f;
        }
    }
}