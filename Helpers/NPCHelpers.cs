using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace Bangarang.Helpers;

public static class NPCHelpers
{
    /// <summary>Gets the closest hostile NPC within the range of that position</summary>
    /// <param name="position">The position, should be the center of the search and usually the center of another entity</param>
    /// <param name="range">The range measured in units, 1 tile is 16 units</param>
    /// <param name="careAboutLineOfSight">Whether the function should check Collision.CanHit</param>
    /// <param name="excludedNPCs">The whoAmI fields of any NPCs that are excluded from the search</param>
    /// <returns>Returns the closest NPC. Returns null if no NPC is found</returns>
    public static bool TryGetClosestEnemy(Vector2 position, float range, out NPC closestNPC, bool careAboutLineOfSight = true, List<int> excludedNPCs = null) {
        closestNPC = null;
        float closestNPCDistance = float.PositiveInfinity;
        excludedNPCs ??= new List<int>();

        for (int i = 0; i < Main.maxNPCs; i++) {
            NPC npc = Main.npc[i];

            float distance = Vector2.Distance(position, npc.Center);
            if (!npc.CanBeChasedBy() || !npc.WithinRange(position, range) || excludedNPCs.Contains(npc.whoAmI) || distance > closestNPCDistance) {
                continue;
            }

            // Double guard clause since collision check is pretty performance intensive
            if (careAboutLineOfSight && !CollisionHelpers.CanHit(npc, position)) {
                continue;
            }

            closestNPC = npc;
            closestNPCDistance = distance;
        }

        return closestNPC is not null;
    }

    /// <summary>Gets a list of NPCs within the range of that position</summary>
    /// <param name="position">The position, should be the center of the search and usually the center of another entity</param>
    /// <param name="range">The range measured in units, 1 tile is 16 units</param>
    /// <param name="careAboutLineOfSight">Whether the function should check Collision.CanHit</param>
    /// <param name="excludedNPCs">The whoAmI fields of any NPCs that are excluded from the search</param>
    /// <returns>A list of NPCs within range of the position</returns>
    public static List<NPC> GetNearbyEnemies(Vector2 position, float range, bool careAboutLineOfSight = true, List<int> excludedNPCs = null) {
        List<NPC> npcs = new();
        excludedNPCs ??= new List<int>();

        for (int i = 0; i < Main.npc.Length; i++) {
            NPC npc = Main.npc[i];

            if (!npc.CanBeChasedBy() || !npc.WithinRange(position, range) || excludedNPCs.Contains(npc.whoAmI)) {
                continue;
            }

            if (!careAboutLineOfSight || Collision.CanHit(position, 1, 1, npc.position, npc.width, npc.height)) {
                npcs.Add(npc);
            }
        }

        return npcs;
    }
}