using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace Bangarang {
    public static class Helpers {
        /// <summary>Adds an item to a shop</summary>
        /// <param name="shop">The <c>shop</c> parameter of <c>SetupShop</c></param>
        /// <param name="nextSlot">The <c>nextSlot</c> parameter of <c>SetupShop</c>, no need to increment this yourself as this function does that if needed</param>
        /// <param name="itemType">The type of the item being inserted</param>
        /// <param name="after">The predicate an Item must match for the added item to be inserted after it</param>
        /// <param name="defaultIndex">The default index if an item with type <c>itemAfterType</c> isn't found</param>
        public static void AddToShop(ref Chest shop, ref int nextSlot, int itemType, Predicate<Item> after, int defaultIndex) {
            // Get our inventory as a list
            List<Item> inventory = shop.item.ToList();

            // Get our correct index
            Item itemAfter = inventory.FirstOrDefault(i => after.Invoke(i));
            int index = defaultIndex;
            if (itemAfter != null)
                index = inventory.IndexOf(itemAfter) + 1;

            // Check itemType isn't already in our shop
            Item item = inventory.FirstOrDefault(i => i.type == itemType);
            if (item != null) {
                // Move it if it is
                inventory.Remove(item);
                inventory.Insert(index, item);

                // Reassign our item array as ToList doesnt provide a reference
                shop.item = inventory.ToArray();

                return;
            }

            // Add our item since it isn't here
            inventory.Insert(index, new(itemType));
            inventory[index].isAShopItem = true;
            nextSlot++;

            shop.item = inventory.ToArray();
        }

        /// <summary>Gets the closest hostile NPC within the range of that position</summary>
        /// <param name="position">The position, should be the center of the search and usually the center of another entity</param>
        /// <param name="range">The range measured in units, 1 tile is 16 units</param>
        /// <param name="careAboutLineOfSight">Whether the function should check Collision.CanHit</param>
        /// <param name="careAboutCanBeChased">Whether the function should check npc.chaseable</param>
        /// <param name="excludedNPCs">The whoAmI fields of any NPCs that are excluded from the search</param>
        /// <returns>Returns the closest NPC. Returns null if no NPC is found</returns>
        public static NPC GetClosestEnemy(Vector2 position, float range, bool careAboutLineOfSight, bool careAboutCanBeChased, List<int> excludedNPCs = null) {
            NPC closestNPC = null;
            float rangeSquared = range * range;
            if (excludedNPCs == null)
                excludedNPCs = new List<int>();

            for (int i = 0; i < Main.npc.Length; i++) {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.CountsAsACritter || npc.friendly || !npc.immortal || excludedNPCs.Contains(npc.whoAmI)) {
                    continue;
                }

                float distanceSquared = Vector2.DistanceSquared(position, npc.Center);
                bool canSee = careAboutLineOfSight ? Collision.CanHit(position, 1, 1, npc.position, npc.width, npc.height) : true;
                bool canBeChased = careAboutCanBeChased ? npc.chaseable : true;
                if (distanceSquared < rangeSquared && canSee && canBeChased) {
                    closestNPC = npc;
                    rangeSquared = distanceSquared;
                }
            }

            return closestNPC;
        }
    }
}