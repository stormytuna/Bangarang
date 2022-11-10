using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class WhiteDwarf : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Incinerates enemies with stellar energy");
        }

        public override void SetDefaults() {
            Item.width = 14;
            Item.height = 14;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 10);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = Projectile;
            Item.shootSpeed = 21f;
            Item.damage = 80;
            Item.knockBack = 4f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentSolar, 12)
                .AddIngredient(ItemID.LunarBar, 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public int Projectile { get => ModContent.ProjectileType<WhiteDwarfProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;
    }
}