using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Bangarang.Common.Configs {
    public class ServerConfig : ModConfig {
        public static ServerConfig Instance;

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("[i:4818] Vanilla boomerang changes")]
        [DefaultValue(true)]
        public bool VanillaChanges { get; set; }

        [Label("$Mods.Bangarang.Config.ModdedBoomerangs")]
        [DefaultValue(true)]
        public bool ModdedBoomerangs { get; set; }

        [Label("$Mods.Bangarang.Config.ModdedAccessories")]
        [DefaultValue(true)]
        public bool ModdedAccessory { get; set; }

        [Label("$Mods.Bangarang.Config.AutoSupport")]
        [Tooltip("This will use data from the projectile to guess which projectiles are boomerangs\nThis may have some unintended behaviour with some mods")]
        [DefaultValue(false)]
        public bool AssumeModdedBoomerangs { get; set; }
    }
}