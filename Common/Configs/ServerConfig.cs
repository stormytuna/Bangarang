using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Bangarang.Common.Configs {
    public class ServerConfig : ModConfig {
        public static ServerConfig Instance;

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Vanilla boomerang changes")]
        [DefaultValue(true)]
        public bool VanillaChanges { get; set; }

        [Label("Modded boomerangs")]
        [DefaultValue(true)]
        public bool ModdedBoomerangs { get; set; }

        [Label("Modded accessories")]
        [DefaultValue(true)]
        public bool ModdedAccessory { get; set; }
    }
}