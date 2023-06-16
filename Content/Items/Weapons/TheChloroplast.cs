using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class TheChloroplast : ModItem
{
	public override void SetStaticDefaults() {
		Tooltip.SetDefault("Bursts into stingers at its apex");
	}

	public override void SetDefaults() {
		Item.width = 54;
		Item.height = 54;
		Item.rare = ItemRarityID.Lime;
		Item.value = Item.sellPrice(gold: 5);

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 30;
		Item.useTime = 30;
		Item.autoReuse = true;
		Item.UseSound = SoundID.Item1;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.shoot = Projectile;
		Item.shootSpeed = 20f;
		Item.damage = 50;
		Item.knockBack = 12f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public int Projectile => ModContent.ProjectileType<TheChloroplastProj>();

	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 3;

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.ChlorophyteBar, 14)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}