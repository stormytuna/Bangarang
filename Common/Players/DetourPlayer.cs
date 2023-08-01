using Bangarang.Common.Systems;
using Terraria;
using Terraria.ModLoader;
using Item = Terraria.Item;

namespace Bangarang.Common.Players;

public class DetourPlayer : ModPlayer
{
    public override void Load() {
        Terraria.On_Player.ItemCheck_CheckCanUse += CheckCanUseDetour;
        ;
    }

    private bool CheckCanUseDetour(On_Player.orig_ItemCheck_CheckCanUse orig, Player self, Item sItem) {
        bool ret = orig(self, sItem);

        if (BoomerangInfoSystem.BoomerangInfoDict.ContainsKey(sItem.type)) {
            BoomerangInfoSystem.BoomerangInfo boomerangInfo = BoomerangInfoSystem.BoomerangInfoDict[sItem.type];
            int extraBoomerangs = self.GetModPlayer<BangarangPlayer>().ExtraBoomerangs;
            // -2 == just return our orig
            if (boomerangInfo.NumBoomerangs == -2) {
                return boomerangInfo.CanUseItemFunc is not null ? boomerangInfo.CanUseItemFunc(self, sItem, extraBoomerangs) : ret;
            }

            // -1 == explicitly infinite boomerangs
            if (boomerangInfo.NumBoomerangs == -1) {
                return boomerangInfo.CanUseItemFunc is null || boomerangInfo.CanUseItemFunc(self, sItem, extraBoomerangs);
            }

            // Gets our max owned projectiles for any boomerang in our boomerang info
            int ownedProj = 0;
            foreach (int projType in boomerangInfo.ProjectileTypes) {
                if (ownedProj < self.ownedProjectileCounts[projType]) {
                    ownedProj = self.ownedProjectileCounts[projType];
                }
            }

            if (ownedProj < boomerangInfo.NumBoomerangs + extraBoomerangs) {
                return boomerangInfo.CanUseItemFunc is null || boomerangInfo.CanUseItemFunc(self, sItem, extraBoomerangs);
            }
        }

        return ret;
    }
}