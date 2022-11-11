using Bangarang.Common.Configs;
using Bangarang.Common.Systems;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.GlobalItems {
    public class BangarangGlobalItem : GlobalItem {
        public override void SetDefaults(Item item) {
            if (ArraySystem.ProjectilesThatAreBoomerangs.Contains(item.shoot) || (ServerConfig.Instance.AssumeModdedBoomerangs && ContentSamples.ProjectilesByType[item.shoot].aiStyle == 3 && item.CountsAsClass(DamageClass.Melee))) {
                item.autoReuse = true;
            }
        }
    }
}