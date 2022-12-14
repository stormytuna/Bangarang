using Bangarang.Common.Systems;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Bangarang.Common.GlobalItems {
    public class BangarangGlobalItem : GlobalItem {
        public override void SetDefaults(Item item) {
            if (ArraySystem.ProjectilesThatAreBoomerangs.Contains(item.shoot)) {
                item.autoReuse = true;
            }
        }
    }
}