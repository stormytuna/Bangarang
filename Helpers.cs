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
    }
}