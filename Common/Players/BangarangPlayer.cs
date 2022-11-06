using Terraria.ModLoader;

namespace Bangarang.Common.Players {
    public class BangarangPlayer : ModPlayer {
        public int ExtraBoomerangs { get; set; }

        public override void ResetEffects() {
            ExtraBoomerangs = 0;
        }
    }
}