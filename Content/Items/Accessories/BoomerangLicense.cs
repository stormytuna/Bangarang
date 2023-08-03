using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Accessories;

public class BoomerangLicense : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedAccessory;

    public override void SetDefaults() {
        Item.width = 38;
        Item.height = 28;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(gold: 5);

        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs++;
}

public class BoomerangLicenseGNPC : GlobalNPC
{
    public override void SetupTravelShop(int[] shop, ref int nextSlot) {
        bool downedAnyBoss = NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedDeerclops || NPC.downedQueenBee || NPC.downedSlimeKing || Main.hardMode;
        if (!downedAnyBoss || !Main.rand.NextBool(10) || !ServerConfig.Instance.ModdedAccessory) {
            return;
        }

        shop[nextSlot] = ModContent.ItemType<BoomerangLicense>();
        nextSlot++;
    }
}