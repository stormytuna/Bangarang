using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class Teslarang : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Fires lightning at nearby enemies");
        }

        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 46;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 6);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = Projectile;
            Item.shootSpeed = 14f;
            Item.damage = 50;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 8)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public int Projectile { get => ModContent.ProjectileType<TeslarangProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;
    }
}