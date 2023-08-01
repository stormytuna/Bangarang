using Terraria.ModLoader;

namespace Bangarang.Common.Configs;

public abstract class ConfigItem : ModItem
{
	public override void SetStaticDefaults() {
		// Tooltip.SetDefault("Unobtainable\nJust here to be a config item chat tag");
	}

	public override void SetDefaults() {
		Item.width = 26;
		Item.height = 26;
	}
}

public class CONFIG_Phylactery : ConfigItem { }

public class CONFIG_Cog : ConfigItem { }

public class CONFIG_ChromaticCrux : ConfigItem { }