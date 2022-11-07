using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class Bonerang : ModItem {
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

            Item.shoot = Projectile;
            Item.shootSpeed = 14f;
            Item.damage = 28;
            Item.crit = 4;
            Item.knockBack = 6f;
            Item.DamageType = DamageClass.Melee;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 30)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public int Projectile { get => ModContent.ProjectileType<BonerangProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;
    }
}