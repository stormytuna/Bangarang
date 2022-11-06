using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs {
    public class BloodyMacheteGI : GlobalItem {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.BloodyMachete;

        public override void SetDefaults(Item item) {
            item.autoReuse = true;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            int index = tooltips.FindLastIndex(t => t.Mod == "Terraria");
            tooltips.Insert(index + 1, new(Mod, "Tooltip0", "Increases in damage while airborne"));
        }
    }

    public class BloodyMacheteGP : GlobalProjectile {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.BloodyMachete;

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            // Bloody Machete uses ai[0] as a frame counter in vanilla so we can just reuse that
            // Should get us a value between 0 and 1 when the frame counter is between 30 and 60
            float lerpAmount = (MathHelper.Clamp(projectile.ai[1], 30f, 60f) - 30f) / 30f;
            float damageMult = MathHelper.Lerp(1f, 1.5f, lerpAmount);
            damage = (int)((float)damage * damageMult);
        }
    }

    public class BloodyMacheteCharkramGNPC : GlobalNPC {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Merchant;

        public override void SetupShop(int type, Chest shop, ref int nextSlot) {
            if (!Main.dayTime) {
                Helpers.AddToShop(ref shop, ref nextSlot, ItemID.BloodyMachete, i => i.type == ItemID.Shuriken, 12);
            }
        }
    }
}
