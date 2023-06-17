using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class Bonerang : ModItem
{
	public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

	public override void SetStaticDefaults() {
		Tooltip.SetDefault("Breaks into bone shards on impact");
	}

	public override void SetDefaults() {
		Item.width = 24;
		Item.height = 36;
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.sellPrice(gold: 2);

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.autoReuse = true;
		Item.UseSound = SoundID.Item1;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.shoot = ModContent.ProjectileType<BonerangProj>();
		;
		Item.shootSpeed = 14f;
		Item.damage = 28;
		Item.knockBack = 6f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Bone, 90)
			.AddTile(TileID.Anvils)
			.Register();
	}

	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;
}