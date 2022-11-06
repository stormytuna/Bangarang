using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Systems {
    public class ArraySystem : ModSystem {
        private static int[] _fruitcakeChakramDebuffs = new[] {
            BuffID.Bleeding,
            BuffID.Confused,
            BuffID.CursedInferno,
            BuffID.Ichor,
            BuffID.Frostburn,
            BuffID.OnFire,
            BuffID.Poisoned,
            BuffID.ShadowFlame
        };

        private static int[] _itemsThatShootBoomerangs = new int[] {
            ItemID.WoodenBoomerang,
            ItemID.EnchantedBoomerang,
            ItemID.FruitcakeChakram,
            ItemID.BloodyMachete,
            ItemID.Shroomerang,
            ItemID.IceBoomerang,
            ItemID.ThornChakram,
            ItemID.CombatWrench,
            ItemID.Flamarang,
            //ItemID.Bananarang // Technically making this impossible to acquire. TODO: in 1.4.4 remove the added light disc
            ItemID.FlyingKnife,
            ItemID.BouncingShield,
            //ItemID.LightDisc // Also technically removing this, TODO: in 1.4.4 remove the added light disc
            ItemID.PaladinsHammer,
            ItemID.PossessedHatchet
            // TODO: Add modded boomerangs to here
        };

        public static int[] FruitcakeChakramDebuffs { get => _fruitcakeChakramDebuffs; }

        public static int[] ItemsThatShootBoomerangs { get => _itemsThatShootBoomerangs; }

        public override void Unload() {
            _fruitcakeChakramDebuffs = null;
        }
    }
}