using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.GameContent.ItemDropRules.Chains;

namespace Bangarang.Content.Items.Weapons {
    public class ChromaticCrux : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Leaves a homing rainbow chakram when it hits an enemy");
        }

        public override void SetDefaults() {
            Item.width = 30;
            Item.height = 33;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = Projectile;
            Item.shootSpeed = 18f;
            Item.damage = 58;
            Item.knockBack = 7f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public int Projectile { get => ModContent.ProjectileType<ChromaticCruxProj>(); }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Projectile] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 3;
    }

    public class ChromaticCruxGlobalNPC : GlobalNPC {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            if (npc.type == NPCID.HallowBoss) {
                foreach (var rule in npcLoot.Get()) {
                    if (rule is LeadingConditionRule leadingConditionRule && leadingConditionRule.condition is Conditions.NotExpert) {
                        foreach (var chain in leadingConditionRule.ChainedRules) {
                            if (chain is TryIfSucceeded chainRule && chainRule.RuleToChain is OneFromOptionsDropRule oneFromOptions) {
                                var original = oneFromOptions.dropIds.ToList();
                                original.Add(ModContent.ItemType<ChromaticCrux>());
                                oneFromOptions.dropIds = original.ToArray();
                            }
                        }
                    }
                }
            }
        }
    }

    public class ChromaticCruxGlobalItem : GlobalItem {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
            if (item.type == ItemID.FairyQueenBossBag) {
                foreach (var rule in itemLoot.Get()) {
                    if (rule is OneFromOptionsNotScaledWithLuckDropRule oneFromOptions) {
                        var original = oneFromOptions.dropIds.ToList();
                        original.Add(ModContent.ItemType<ChromaticCrux>());
                        oneFromOptions.dropIds = original.ToArray();
                    }
                }
            }
        }
    }
}