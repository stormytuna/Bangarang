using System.Collections.Generic;
using Bangarang.Common.Configs;
using Bangarang.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class TeslarangProj : Boomerang
{
    public override void SetStaticDefaults() {
        // DisplayName.SetDefault("Teslarang");
    }

    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Projectile.width = 38;
        Projectile.height = 38;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;

        Projectile.tileCollide = true;

        ReturnSpeed = 19f;
        HomingOnOwnerStrength = 1.5f;
        TravelOutFrames = 30;
        DoTurn = true;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        if (Projectile.ai[0] == 0f) {
            LightningStrike(target.whoAmI, target.Center, damageDone);
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

        if (Projectile.ai[0] == 0f) {
            LightningStrike(-1, Projectile.Center, (int)Owner.GetTotalDamage(Projectile.DamageType).ApplyTo(Projectile.damage));
        }

        return base.OnTileCollide(oldVelocity);
    }

    private void LightningStrike(int whoAmIToIgnore, Vector2 startPos, int damage) {
        List<NPC> closeNPCs = NPCHelpers.GetNearbyEnemies(startPos, 16f * 16f, true, new List<int> {
            whoAmIToIgnore
        });

        int numLightning = (int)MathHelper.Clamp(closeNPCs.Count, 0f, 3f);
        for (int i = 0; i < numLightning; i++) {
            Main.LocalPlayer.ApplyDamageToNPC(closeNPCs[i], damage / 3, 0f, 0, false);
            LightningHelper.MakeDust(startPos, closeNPCs[i].Center);
        }
    }
}

// Code from here
// https://gamedevelopment.tutsplus.com/tutorials/how-to-generate-shockingly-good-2d-lightning-effects--gamedev-2681
// Thank fuck for this post
public class LightningHelper
{
    public static void MakeDust(Vector2 source, Vector2 dest) {
        List<Vector2> dustPoints = CreateBolt(source, dest);
        foreach (Vector2 point in dustPoints) {
            Dust d = Dust.NewDustPerfect(point, DustID.Electric, Scale: 0.8f);
            d.noGravity = true;
            d.velocity = Vector2.Zero;
        }
    }

    public static List<Vector2> CreateBolt(Vector2 source, Vector2 dest) {
        List<Vector2> results = new();
        Vector2 tangent = dest - source;
        Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
        float length = tangent.Length();

        List<float> positions = new() {
            0
        };

        for (int i = 0; i < length; i++) {
            positions.Add(Main.rand.NextFloat(0f, 1f));
        }

        positions.Sort();

        const float Sway = 1000;
        const float Jaggedness = 1 / Sway;

        Vector2 prevPoint = source;
        float prevDisplacement = 0f;
        for (int i = 1; i < positions.Count; i++) {
            float pos = positions[i];

            // used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
            float scale = length * Jaggedness * (pos - positions[i - 1]);

            // defines an envelope. Points near the middle of the bolt can be further from the central line.
            float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

            float displacement = Main.rand.NextFloat(-Sway, Sway);
            displacement -= (displacement - prevDisplacement) * (1 - scale);
            displacement *= envelope;

            Vector2 point = source + (pos * tangent) + (displacement * normal);
            results.Add(point);
            prevPoint = point;
            prevDisplacement = displacement;
        }

        results.Add(prevPoint);

        return results;
    }
}