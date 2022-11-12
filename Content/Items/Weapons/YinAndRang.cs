using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class YinAndRang : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Releases two homing shards at its apex");
        }

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

            Item.shoot = Projectile;
            Item.shootSpeed = 16f;
            Item.damage = 38;
            Item.knockBack = 8f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.LightShard)
                .AddIngredient(ItemID.DarkShard)
                .AddIngredient(ItemID.TitaniumBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public int Projectile { get => ModContent.ProjectileType<YinAndRangProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 2;
    }
}