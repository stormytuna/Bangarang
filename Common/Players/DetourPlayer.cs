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

		if (BoomerangInfoSystem.BoomerangInfoDict.ContainsKey(sItem.type)) {
			BoomerangInfoSystem.BoomerangInfo bi = BoomerangInfoSystem.BoomerangInfoDict[sItem.type];
			int extraBoomerangs = self.GetModPlayer<BangarangPlayer>().ExtraBoomerangs;
			// -2 == just return our orig
			if (bi.NumBoomerangs == -2) {
				if (bi.CanUseItemFunc is not null) {
					return bi.CanUseItemFunc(self, sItem, extraBoomerangs);
				}

				return ret;
			}

			// -1 == explicitly infinite boomerangs
			if (bi.NumBoomerangs == -1) {
				if (bi.CanUseItemFunc is not null) {
					return bi.CanUseItemFunc(self, sItem, extraBoomerangs);
				}

				return true;
			}

			// Gets our max owned projectiles for any boomerang in our boomerang info
			int ownedProj = 0;
			foreach (int projType in bi.ProjectileTypes) {
				if (ownedProj < self.ownedProjectileCounts[projType]) {
					ownedProj = self.ownedProjectileCounts[projType];
				}
			}

			if (ownedProj < bi.NumBoomerangs + extraBoomerangs) {
				if (bi.CanUseItemFunc is not null) {
					return bi.CanUseItemFunc(self, sItem, extraBoomerangs);
				}

				return true;
			}
		}

		return ret;
	}
}