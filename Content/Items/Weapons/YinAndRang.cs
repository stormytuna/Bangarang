using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class YinAndRang : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Item.width = 40;
        Item.height = 40;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 1, silver: 50);

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;
        Item.noMelee = true;
        Item.noUseGraphic = true;

        Item.shoot = ModContent.ProjectileType<YinAndRangProj>();
        Item.shootSpeed = 16f;
        Item.damage = 38;
        Item.knockBack = 8f;
        Item.DamageType = DamageClass.MeleeNoSpeed;
    }

    public override void AddRecipes() => CreateRecipe()
            .AddIngredient(ItemID.LightShard)
            .AddIngredient(ItemID.DarkShard)
            .AddIngredient(ItemID.TitaniumBar, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 2;
}