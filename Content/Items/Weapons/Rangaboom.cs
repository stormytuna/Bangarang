using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class Rangaboom : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Starts at its apex and flies to you");
        }

        public override void SetDefaults() {
            Item.width = 42;
            Item.height = 42;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = Projectile;
            Item.shootSpeed = 21f;
            Item.damage = 80;
            Item.knockBack = 8f;
            Item.DamageType = DamageClass.Melee;
        }

        public int Projectile { get => ModContent.ProjectileType<RangaboomProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 1;

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            position += velocity * 25f;
            velocity = -velocity;
        }
    }

    public class RangaboomGlobalNPC : GlobalNPC {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            if (npc.type == NPCID.MartianSaucerCore) {
                foreach (var rule in npcLoot.Get()) {
                    if (rule is OneFromOptionsNotScaledWithLuckDropRule oneFromOptions) {
                        var original = oneFromOptions.dropIds.ToList();
                        original.Add(ModContent.ItemType<Rangaboom>());
                        oneFromOptions.dropIds = original.ToArray();
                    }
                }
            }
        }
    }
}