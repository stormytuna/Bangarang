using Bangarang.Common.Players;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Accessories;

public class SpectralAmulet : ModItem
{
	public override void SetStaticDefaults() {
		Tooltip.SetDefault("Your boomerangs are orbited by a pair of spectral glaives");
	}

	public override void SetDefaults() {
		Item.width = 26;
		Item.height = 26;
		Item.rare = ItemRarityID.LightRed;
		Item.value = Item.sellPrice(gold: 3);

		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual) {
		BangarangPlayer modPlayer = player.GetModPlayer<BangarangPlayer>();
		modPlayer.BoomerangSpectralGlaives = true;
	}
}

public class PhylacteryGlobalNPC : GlobalNPC
{
	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		if (npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored || npc.type == NPCID.RaggedCaster || npc.type == NPCID.RaggedCasterOpenCoat || npc.type == NPCID.DiabolistRed || npc.type == NPCID.DiabolistWhite) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SpectralAmulet>(), 80));
		}
	}
}