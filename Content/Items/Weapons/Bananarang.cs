using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class Bananarang : ModItem
{
	public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.VanillaChanges;

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

		Item.shoot = ProjectileID.Bananarang;
		Item.shootSpeed = 16f;
		Item.damage = 55;
		Item.knockBack = 6.5f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
	}

	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 10;
}

// Replaces vanilla bananarang with ours
public class BananarangGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Clown && ServerConfig.Instance.VanillaChanges;

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		foreach (IItemDropRule rule in npcLoot.Get()) {
			if (rule is CommonDrop drop && drop.itemId == ItemID.Bananarang) {
				npcLoot.Remove(rule);
			}
		}

		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Bananarang>(), 6));
	}
}

public class BananarangGlobalItem : GlobalItem
{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Bananarang && ServerConfig.Instance.VanillaChanges;

	// Hacky fix, if we somehow get a vanilla bananarang, swap it with a modded one
	public override void UpdateInventory(Item item, Player player) {
		item.SetDefaults(ModContent.ItemType<Bananarang>());
	}
}