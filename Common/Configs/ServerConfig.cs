using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Bangarang.Common.Configs;

public class ServerConfig : ModConfig
{
	public static ServerConfig Instance;

	public override ConfigScope Mode => ConfigScope.ServerSide;

	[DefaultValue(true)]
	public bool VanillaChanges { get; set; }

	[DefaultValue(true)]
	public bool ModdedBoomerangs { get; set; }

	[DefaultValue(true)]
	public bool ModdedAccessory { get; set; }
}