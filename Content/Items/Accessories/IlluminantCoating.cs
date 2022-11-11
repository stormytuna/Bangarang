using Bangarang.Common.Players;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Accessories {
    public class IlluminantCoating : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Your boomerangs glow, return faster and have increased knockback");
        }

        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 28;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 1);

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            var modPlayer = player.GetModPlayer<BangarangPlayer>();
            modPlayer.BoomerangReturnSpeedMult += 0.5f;
            modPlayer.BoomerangKnockbackMult += 0.2f;
            modPlayer.BoomerangGlowAndDust = !hideVisual;
        }
    }

    public class IlluminantCoatingGlobalNPC : GlobalNPC {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            if (npc.type == NPCID.IlluminantBat || npc.type == NPCID.IlluminantSlime) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IlluminantCoating>(), 50));
            }
        }
    }
}