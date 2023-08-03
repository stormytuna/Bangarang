using System.Collections.Generic;
using Bangarang.Common.Configs;
using Bangarang.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bangarang.Common.Boomerangs;

public class FruitcakeChakramGlobalItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.FruitcakeChakram && ServerConfig.Instance.VanillaChanges;

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) => tooltips.InsertTooltips(new List<TooltipLine>() {
            new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.Bangarang.Items.FruitcakeChakram.Tooltip0")),
            new TooltipLine(Mod, "Tooltip1", Language.GetTextValue("Mods.Bangarang.Items.FruitcakeChakram.Tooltip1"))
        }, "Material");
}

public class FruitcakeChakramGlobalProjetile : GlobalProjectile
{
    private static int[] fruitcakeChakramDebuffs = { BuffID.Confused, BuffID.CursedInferno, BuffID.Ichor, BuffID.Frostburn, BuffID.OnFire, BuffID.Poisoned, BuffID.ShadowFlame };

    public override void Unload() => fruitcakeChakramDebuffs = null;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.FruitcakeChakram && ServerConfig.Instance.VanillaChanges;

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
        if (Main.rand.NextBool(4)) {
            int buff = Main.rand.Next(fruitcakeChakramDebuffs);
            int time = (int)(Main.rand.NextFloat(1, 3) * 60f);
            target.AddBuff(buff, time);
        }
    }
}

public class FruitcakeChakramDetour : ModSystem
{
    public override void Load() {
        if (ServerConfig.Instance.VanillaChanges) {
            Terraria.On_WorldGen.ShakeTree += ShakeTreeDetour;
        }
    }

    private void ShakeTreeDetour(On_WorldGen.orig_ShakeTree orig, int i, int j) {
        orig(i, j);

        WorldGen.GetTreeBottom(i, j, out int x, out int y);
        TreeTypes treeType = WorldGen.GetTreeType(Main.tile[x, y].TileType);
        if (WorldGen.genRand.NextBool(15) && treeType == TreeTypes.Snow) {
            Item.NewItem(Terraria.WorldGen.GetItemSource_FromTreeShake(i, j), new Rectangle(i * 16, j * 16, 16, 16), ItemID.FruitcakeChakram);
        }
    }
}