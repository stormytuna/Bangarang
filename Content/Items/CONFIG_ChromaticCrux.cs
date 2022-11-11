using Terraria.ModLoader;

namespace Bangarang.Content.Items {
    public class CONFIG_ChromaticCrux : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Unobtainable\nJust here to be a config item chat tag");
        }
        public override void SetDefaults() {
            Item.width = 30;
            Item.height = 30;
        }
    }
}
