using Bangarang.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs {
    public class FruitcakeChakramGI : GlobalItem {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.FruitcakeChakram;

        public override void SetDefaults(Item item) {
            item.autoReuse = true;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            int index = tooltips.FindLastIndex(t => t.Mod == "Terraria");
            tooltips.Insert(index + 1, new(Mod, "Tooltip0", "Inflicts a random debuff on hit"));
            tooltips.Insert(index + 2, new(Mod, "Tooltip1", "'Rainbow effects!'"));
        }
    }

    public class FruitcakeChakramGP : GlobalProjectile {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.FruitcakeChakram;

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) {
            int[] buffs = ArraySystem.FruitcakeChakramDebuffs;

            if (Main.rand.NextBool(4)) {
                int buff = buffs[Main.rand.Next(buffs.Length)];
                int time = (int)(Main.rand.NextFloat(3, 5) * 60f);
                target.AddBuff(buff, time);
            }
        }
    }

    public class FruitcakeCharkramGNPC : GlobalNPC {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Merchant;

        public override void SetupShop(int type, Chest shop, ref int nextSlot) {
            if (Main.dayTime) {
                Helpers.AddToShop(ref shop, ref nextSlot, ItemID.FruitcakeChakram, i => i.type == ItemID.Shuriken, 12);
            }
        }
    }
}