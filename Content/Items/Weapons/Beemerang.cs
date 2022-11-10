using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class Beemerang : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Releases bees on impact");
        }

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 46;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = Projectile;
            Item.shootSpeed = 13f;
            Item.damage = 30;
            Item.knockBack = 8f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.BeeWax, 14)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public int Projectile { get => ModContent.ProjectileType<BeemerangProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;
    }
}