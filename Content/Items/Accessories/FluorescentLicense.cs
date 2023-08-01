using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Accessories;

public class FluorescentLicense : ModItem
{
	public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedAccessory;

	public override void SetStaticDefaults() {
		// Tooltip.SetDefault("Throw an extra boomerang\nYour boomerangs glow, return faster and have increased knockback");
	}

	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 28;
		Item.rare = ItemRarityID.Pink;
		Item.value = Item.sellPrice(gold: 1, silver: 50);

		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual) {
		BangarangPlayer modPlayer = player.GetModPlayer<BangarangPlayer>();
		modPlayer.ExtraBoomerangs++;
		modPlayer.BoomerangReturnSpeedMult += 0.5f;
		modPlayer.BoomerangKnockbackMult += 0.2f;
		modPlayer.BoomerangGlowAndDust = !hideVisual;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<IlluminantCoating>())
			.AddIngredient(ModContent.ItemType<BoomerangLicense>())
			.AddTile(TileID.TinkerersWorkbench)
			.Register();
	}
}