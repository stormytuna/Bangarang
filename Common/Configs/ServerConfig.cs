using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Bangarang.Common.Configs {
    public class ServerConfig : ModConfig {
        public static ServerConfig Instance;

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("General")]

        [Label("Vanilla boomerang changes")]
        [DefaultValue(true)]
        public bool VanillaChanges { get; set; }

        [Label("Modded boomerangs")]
        [DefaultValue(true)]
        public bool ModdedBoomerangs { get; set; }

        [Label("Modded accessories")]
        [DefaultValue(true)]
        public bool ModdedAccessory { get; set; }


        [Header("Auto-compatibility")]

        [Label("Automatically support other mods boomerangs")]
        [Tooltip("This will use data from the projectile to guess which projectiles are boomerangs\nThis may have some unintended behaviour with some mods")]
        [DefaultValue(false)]
        public bool AssumeModdedBoomerangs { get; set; }

        [Label("Auto-support item blacklist")]
        [Tooltip("Adding an item to this list will prevent this mod from assuming it's a boomerang\nOnly add other mods boomerangs to this list")]
        public List<ItemDefinition> AssumeModdedBoomerangsBlacklist { get; set; } = new();
    }
}