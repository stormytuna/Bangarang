using Bangarang.Common.Configs;
using Bangarang.Common.Players;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Items.Weapons;

public class Teslarang : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

    public override void SetDefaults() {
        Item.width = 18;
        Item.height = 46;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.sellPrice(gold: 6);

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;
        Item.noMelee = true;
        Item.noUseGraphic = true;

        Item.shoot = ModContent.ProjectileType<TeslarangProj>();
        Item.shootSpeed = 16f;
        Item.damage = 50;
        Item.knockBack = 5f;
        Item.DamageType = DamageClass.MeleeNoSpeed;
    }

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < player.GetModPlayer<BangarangPlayer>().ExtraBoomerangs + 3;
}

public class TeslarangGlobalNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Frankenstein && ServerConfig.Instance.ModdedBoomerangs;

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) => npcLoot.Add(ItemDropRule.ByCondition(new Conditions.DownedPlantera(), ModContent.ItemType<Teslarang>(), 40));
}