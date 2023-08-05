using System.IO;
using Bangarang.Common.Configs;
using Bangarang.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons;

public class SynapseProj : Boomerang
{
    public override void SetStaticDefaults() => ProjectileID.Sets.CultistIsResistantTo[Type] = true;

    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Projectile.width = 32;
        Projectile.height = 30;
        Projectile.aiStyle = -1;

        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;

        Projectile.tileCollide = true;

        ReturnSpeed = 15f;
        HomingOnOwnerStrength = 1.2f;
        TravelOutFrames = 30;
        DoTurn = true;
    }

    public override string Texture => "Bangarang/Content/Items/Weapons/Synapse";

    private int currentTarget = -1;

    public override void AI() {
        // Only want to home in on enemies while travelling out
        if (Projectile.ai[0] == 0f) {
            // If there's an npc near the boomerang, we want to move towards it
            if (NPCHelpers.TryGetClosestEnemy(Projectile.Center, 20f * 16f, out NPC closestEnemy)) {
                DoTurn = false;
                GeneralHelpers.SmoothHoming(Projectile, closestEnemy.Center, 0.6f, 12f, bufferZone: false);
            } else {
                DoTurn = true;
            }
        }

        // Calling base so we have the default AI provided by our Boomerang class
        base.AI();
    }

    public override void SendExtraAI(BinaryWriter writer) => writer.Write(currentTarget);

    public override void ReceiveExtraAI(BinaryReader reader) => currentTarget = reader.ReadInt32();
}