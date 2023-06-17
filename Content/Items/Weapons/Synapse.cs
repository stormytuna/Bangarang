using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class Synapse : ModItem
{
	public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

	public override void SetStaticDefaults() {
		Tooltip.SetDefault("Chases down nearby enemies");
	}

	public override void SetDefaults() {
		Item.width = 32;
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

		Item.shoot = ModContent.ProjectileType<SynapseProj>();
		Item.shootSpeed = 12f;
		Item.damage = 25;
		Item.knockBack = 8f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.CrimtaneBar, 8)
			.AddIngredient(ItemID.TissueSample, 5)
			.AddTile(TileID.Anvils)
			.Register();
	}
	
	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;
}