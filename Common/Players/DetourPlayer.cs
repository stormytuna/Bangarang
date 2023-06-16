using Bangarang.Common.Systems;
using On.Terraria;
using Terraria.ModLoader;
using Item = Terraria.Item;

namespace Bangarang.Common.Players;

public class DetourPlayer : ModPlayer
{
	public override void Load() {
		On.Terraria.Player.ItemCheck_CheckCanUse += Player_ItemCheck_CheckCanUse;
	}

	private bool Player_ItemCheck_CheckCanUse(Player.orig_ItemCheck_CheckCanUse orig, Terraria.Player self, Item sItem) {
		bool ret = orig(self, sItem);

		if (ArraySystem.BoomerangInfoDict.ContainsKey(sItem.type)) {
			ArraySystem.BoomerangInfo bi = ArraySystem.BoomerangInfoDict[sItem.type];
			int extraBoomerangs = self.GetModPlayer<BangarangPlayer>().ExtraBoomerangs;
			// -2 == just return our orig
			if (bi.numBoomerangs == -2) {
				if (bi.canUseItemFunc is not null) {
					return bi.canUseItemFunc(self, sItem, extraBoomerangs);
				}

				return ret;
			}

			// -1 == explicitly infinite boomerangs
			if (bi.numBoomerangs == -1) {
				if (bi.canUseItemFunc is not null) {
					return bi.canUseItemFunc(self, sItem, extraBoomerangs);
				}

				return true;
			}

			// Gets our max owned projectiles for any boomerang in our boomerang info
			int ownedProj = 0;
			foreach (int projType in bi.projectileTypes) {
				if (ownedProj < self.ownedProjectileCounts[projType]) {
					ownedProj = self.ownedProjectileCounts[projType];
				}
			}

			if (ownedProj < bi.numBoomerangs + extraBoomerangs) {
				if (bi.canUseItemFunc is not null) {
					return bi.canUseItemFunc(self, sItem, extraBoomerangs);
				}

				return true;
			}
		}

		return ret;
	}
}