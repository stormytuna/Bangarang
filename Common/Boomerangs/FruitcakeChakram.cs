using System.Collections.Generic;
using Bangarang.Common.Configs;
using Bangarang.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using WorldGen = On.Terraria.WorldGen;

namespace Bangarang.Common.Boomerangs;

public class FruitcakeChakramGI : GlobalItem
{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.FruitcakeChakram && ServerConfig.Instance.VanillaChanges;

	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		int index = tooltips.FindLastIndex(t => t.Mod == "Terraria");
		tooltips.Insert(index + 1, new TooltipLine(Mod, "Tooltip0", "Inflicts a random debuff on hit"));
		tooltips.Insert(index + 2, new TooltipLine(Mod, "Tooltip1", "'Rainbow effects!'"));
	}
}

public class FruitcakeChakramGP : GlobalProjectile
{
	public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.FruitcakeChakram && ServerConfig.Instance.VanillaChanges;

	public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) {
		int[] buffs = ArraySystem.FruitcakeChakramDebuffs;

		if (Main.rand.NextBool(4)) {
			int buff = buffs[Main.rand.Next(buffs.Length)];
			int time = (int)(Main.rand.NextFloat(1, 3) * 60f);
			target.AddBuff(buff, time);
		}
	}
}

public class FruitcakeChakramDetour : ModSystem
{
	public override void Load() {
		if (ServerConfig.Instance.VanillaChanges) {
			WorldGen.ShakeTree += WorldGen_ShakeTree;
		}
	}

	public override void Unload() {
		if (ServerConfig.Instance.VanillaChanges) {
			WorldGen.ShakeTree -= WorldGen_ShakeTree;
		}
	}

	private void WorldGen_ShakeTree(WorldGen.orig_ShakeTree orig, int i, int j) {
		orig(i, j);

		// Gets our trees bottom tile
		// Copied from vanilla: Terraria.Worldgen.cs:40444
		int x = i;
		int y = j;
		Tile tileSafely = Framing.GetTileSafely(x, y);
		if (tileSafely.TileType == 323) {
			while (y < Main.maxTilesY - 50 && (!tileSafely.HasTile || tileSafely.TileType == 323)) {
				y++;
				tileSafely = Framing.GetTileSafely(x, y);
			}

			return;
		}

		int num = tileSafely.TileFrameX / 22;
		int num2 = tileSafely.TileFrameY / 22;
		if (num == 3 && num2 <= 2) {
			x++;
		} else if (num == 4 && num2 >= 3 && num2 <= 5) {
			x--;
		} else if (num == 1 && num2 >= 6 && num2 <= 8) {
			x--;
		} else if (num == 2 && num2 >= 6 && num2 <= 8) {
			x++;
		} else if (num == 2 && num2 >= 9) {
			x++;
		} else if (num == 3 && num2 >= 9) {
			x--;
		}

		tileSafely = Framing.GetTileSafely(x, y);
		while (y < Main.maxTilesY - 50 && (!tileSafely.HasTile || TileID.Sets.IsATreeTrunk[tileSafely.TileType] || tileSafely.TileType == 72)) {
			y++;
			tileSafely = Framing.GetTileSafely(x, y);
		}

		TreeTypes treeType = Terraria.WorldGen.GetTreeType(Main.tile[x, y].TileType);
		if (Terraria.WorldGen.genRand.NextBool(15) && treeType == TreeTypes.Snow) {
			Item.NewItem(Terraria.WorldGen.GetItemSource_FromTreeShake(i, j), new Rectangle(i * 16, j * 16, 16, 16), ItemID.FruitcakeChakram);
		}
	}
}