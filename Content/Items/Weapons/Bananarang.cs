using Bangarang.Common.Players;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons {
    public class Bananarang : ModItem {
        public override void SetDefaults() {
            Item.width = 14;
            Item.height = 28;
            Item.rare = ItemRarityID.Pink;
            Item.value = 75000;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 11;
            Item.useTime = 11;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = Projectile;
            Item.shootSpeed = 16f;
            Item.damage = 55;
            Item.knockBack = 6.5f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public int Projectile { get => ProjectileID.Bananarang; }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 10;
    }

    // Replaces vanilla bananarang with ours
    public class BananarangGlobalNPC : GlobalNPC {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            if (npc.type == NPCID.Clown) {
                foreach (var rule in npcLoot.Get()) {
                    if (rule is CommonDrop drop && drop.itemId == ItemID.Bananarang) {
                        npcLoot.Remove(rule);
                    }
                }

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Bananarang>(), 6));
            }
        }
    }
}