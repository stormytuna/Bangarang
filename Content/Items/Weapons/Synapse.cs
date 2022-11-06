using Bangarang.Common.Players;
using Bangarang.Content.Projectile.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class Synapse : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Homes in on nearby enemies");
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1, silver: 50);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;

            Item.shoot = Projectile;
            Item.shootSpeed = 12f;
            Item.damage = 25;
            Item.knockBack = 8f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 8)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public int Projectile { get => ModContent.ProjectileType<SynapseProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;
    }
}