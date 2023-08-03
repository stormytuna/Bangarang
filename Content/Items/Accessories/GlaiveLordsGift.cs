using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Accessories;

[LegacyName("RangersTalisman")]
public class GlaiveLordsGift : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedAccessory;

    public override void SetDefaults() {
        Item.width = 18;
        Item.height = 28;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(gold: 4);

        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        BangarangPlayer modPlayer = player.GetModPlayer<BangarangPlayer>();
        modPlayer.ExtraBoomerangs++;
        modPlayer.BoomerangReturnSpeedMult += 0.5f;
        modPlayer.BoomerangKnockbackMult += 0.2f;
        modPlayer.BoomerangGlowAndDust = !hideVisual;
        modPlayer.BoomerangSpectralGlaives = true;
    }

    public override void AddRecipes() => CreateRecipe()
            .AddIngredient(ModContent.ItemType<FluorescentLicense>())
            .AddIngredient(ModContent.ItemType<SpectralAmulet>())
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
}