using System.Collections.Generic;
using System.Linq;
using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.GameContent.ItemDropRules.Chains;

namespace Bangarang.Content.Items.Weapons;

public class ChromaticCrux : ModItem
{
	public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

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

		Item.shoot = ModContent.ProjectileType<ChromaticCruxProj>();
		;
		Item.shootSpeed = 18f;
		Item.damage = 58;
		Item.knockBack = 7f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 3;
}

public class ChromaticCruxGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.HallowBoss && ServerConfig.Instance.ModdedBoomerangs;

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		foreach (IItemDropRule rule in npcLoot.Get()) {
			if (rule is LeadingConditionRule leadingConditionRule && leadingConditionRule.condition is Conditions.NotExpert) {
				foreach (IItemDropRuleChainAttempt chain in leadingConditionRule.ChainedRules) {
					if (chain is TryIfSucceeded chainRule && chainRule.RuleToChain is OneFromOptionsDropRule oneFromOptions) {
						List<int> original = oneFromOptions.dropIds.ToList();
						original.Add(ModContent.ItemType<ChromaticCrux>());
						oneFromOptions.dropIds = original.ToArray();
					}
				}
			}
		}
	}
}

public class ChromaticCruxGlobalItem : GlobalItem
{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.FairyQueenBossBag && ServerConfig.Instance.ModdedBoomerangs;

	public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
		foreach (IItemDropRule rule in itemLoot.Get()) {
			if (rule is OneFromOptionsNotScaledWithLuckDropRule oneFromOptions) {
				List<int> original = oneFromOptions.dropIds.ToList();
				original.Add(ModContent.ItemType<ChromaticCrux>());
				oneFromOptions.dropIds = original.ToArray();
			}
		}
	}
}