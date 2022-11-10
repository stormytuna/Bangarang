using Bangarang.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Accessories {
    public class BoomerangLicense : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Throw an extra boomerang");
        }

        public override void SetDefaults() {
            Item.width = 38;
            Item.height = 28;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 5);

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs++;
        }
    }
}