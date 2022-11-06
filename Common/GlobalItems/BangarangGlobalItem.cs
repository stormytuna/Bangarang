using Bangarang.Common.Systems;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Bangarang.Common.GlobalItems {
    public class BangarangGlobalItem : GlobalItem {
        public override void SetDefaults(Item item) {
            if (ArraySystem.ItemsThatShootBoomerangs.Contains(item.type)) {
                item.autoReuse = true;
            }
        }
    }
}