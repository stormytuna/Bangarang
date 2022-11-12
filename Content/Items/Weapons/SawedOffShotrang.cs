using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class SawedOffShotrang : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Fires a shotgun blast at its apex\n'Who said guns were ranged weapons?'");
        }

        public override void SetDefaults() {
            Item.width = 38;
            Item.height = 14;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 3);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = Projectile;
            Item.shootSpeed = 14f;
            Item.damage = 34;
            Item.knockBack = 6f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Shotgun)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public int Projectile { get => ModContent.ProjectileType<SawedOffShotrangProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 2;
    }
}