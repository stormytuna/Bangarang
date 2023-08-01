using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class ShadeChakram : ModItem
{
	public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

	public override void SetStaticDefaults() {
		// Tooltip.SetDefault("Exceptionally quick");
	}

	public override void SetDefaults() {
		Item.width = 30;
		Item.height = 30;
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.sellPrice(gold: 1, silver: 50);

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.autoReuse = true;
		Item.UseSound = SoundID.Item1;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.shoot = ModContent.ProjectileType<ShadeChakramProj>();
		Item.shootSpeed = 24f;
		Item.damage = 27;
		Item.knockBack = 8f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.DemoniteBar, 8)
			.AddIngredient(ItemID.ShadowScale, 5)
			.AddTile(TileID.Anvils)
			.Register();
	}

	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;
}