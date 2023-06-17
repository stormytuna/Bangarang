using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class LightDisc : ModItem
{
	public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.VanillaChanges;

	public override void SetDefaults() {
		Item.width = 24;
		Item.height = 24;
		Item.rare = ItemRarityID.Pink;
		Item.value = Item.sellPrice(gold: 6);

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.autoReuse = true;
		Item.UseSound = SoundID.Item1;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.shoot = ProjectileID.LightDisc;
		Item.shootSpeed = 13f;
		Item.damage = 57;
		Item.knockBack = 6.5f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.HallowedBar, 12)
			.AddIngredient(ItemID.SoulofLight, 9)
			.AddIngredient(ItemID.SoulofMight, 15)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}

	// TODO: 1.4.4 - change this to + 6
	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 5;
}

public class LightDiscRecipeSystem : ModSystem
{
	public override void PostAddRecipes() {
		for (int i = 0; i < Main.recipe.Length; i++) {
			if (Main.recipe[i].createItem.type == ItemID.LightDisc) {
				Main.recipe[i].DisableRecipe();
			}
		}
	}
}

public class LightDiscGlobalItem : GlobalItem
{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.LightDisc && ServerConfig.Instance.VanillaChanges;

	// Hacky fix, if we somehow get a vanilla light disc, swap it with a modded one
	public override void UpdateInventory(Item item, Player player) {
		item.SetDefaults(ModContent.ItemType<LightDisc>());
	}
}