using System.Collections.Generic;
using System.Linq;
using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class Rangaboom : ModItem
{
	public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

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

		Item.shoot = ModContent.ProjectileType<RangaboomProj>();
		Item.shootSpeed = 25f;
		Item.damage = 75;
		Item.knockBack = 8f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 5;

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position += velocity * 25f;
		velocity = -velocity;
	}
}

public class RangaboomGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.MartianSaucerCore && ServerConfig.Instance.ModdedBoomerangs;

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		foreach (IItemDropRule rule in npcLoot.Get()) {
			if (rule is OneFromOptionsNotScaledWithLuckDropRule oneFromOptions) {
				List<int> original = oneFromOptions.dropIds.ToList();
				original.Add(ModContent.ItemType<Rangaboom>());
				oneFromOptions.dropIds = original.ToArray();
			}
		}
	}
}