using System.Linq;
using Bangarang.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace Bangarang.Common.GlobalItems;

public class AutoreuseBoomerangsGlobalItem : GlobalItem
{
	public override void SetDefaults(Item item) {
		if (BoomerangInfoSystem.ProjectilesThatAreBoomerangs.Contains(item.shoot)) {
			item.autoReuse = true;
		}
	}
}