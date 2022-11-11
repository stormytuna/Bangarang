using Bangarang.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace Bangarang.Common.Players {
    public class DetourPlayer : ModPlayer {
        public override void Load() {
            On.Terraria.Player.ItemCheck_CheckCanUse += Player_ItemCheck_CheckCanUse;
        }
        public override void Unload() {
            On.Terraria.Player.ItemCheck_CheckCanUse -= Player_ItemCheck_CheckCanUse;
        }

        private bool Player_ItemCheck_CheckCanUse(On.Terraria.Player.orig_ItemCheck_CheckCanUse orig, Player self, Item sItem) {
            bool ret = orig(self, sItem);

            foreach (var kvp in ArraySystem.BoomerangMaxOutCount) {
                if (sItem.type == kvp.Key && kvp.Value != -1) {
                    return self.ownedProjectileCounts[sItem.shoot] < kvp.Value + self.GetModPlayer<BangarangPlayer>().ExtraBoomerangs;
                }
            }

            return ret;
        }
    }
}