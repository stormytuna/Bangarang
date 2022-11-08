using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class LightDisc : ModItem {
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

            Item.shoot = Projectile;
            Item.shootSpeed = 13f;
            Item.damage = 57;
            Item.crit = 4;
            Item.knockBack = 6.5f;
            Item.DamageType = DamageClass.Melee;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.SoulofLight, 9)
                .AddIngredient(ItemID.SoulofMight, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public int Projectile { get => ProjectileID.LightDisc; }
    }

    public class LightDiscRecipeSystem : ModSystem {
        public override void PostAddRecipes() {
            for (int i = 0; i < Main.recipe.Length; i++) {
                if (Main.recipe[i].createItem.type == ItemID.LightDisc) {
                    Main.recipe[i].DisableRecipe();
                }
            }
        }
    }
}