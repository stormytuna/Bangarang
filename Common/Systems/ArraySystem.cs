using Bangarang.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria;
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

        private static Dictionary<int, int> _boomerangMaxOutCount = new() {
            { ItemID.WoodenBoomerang, 1 },
            { ItemID.EnchantedBoomerang, 1 },
            { ItemID.FruitcakeChakram, 1 },
            { ItemID.BloodyMachete, 1 },
            { ItemID.Shroomerang, 1 },
            { ItemID.IceBoomerang, 1 },
            { ItemID.ThornChakram, 1 },
            { ItemID.CombatWrench, 1 },
            { ItemID.Flamarang, 1 },
            //{ ItemID.Bananarang, 10 }, // TODO: 1.4.4 remove this comment
            { ItemID.FlyingKnife, 1 },
            { ItemID.BouncingShield, 1 },
            //{ ItemID.LightDisc, 5 }, // TODO: 1.4.4 remove this comment
            { ItemID.PaladinsHammer, 1 },
            { ItemID.PossessedHatchet, 1 },
            // TODO: Add modded boomerangs to this
            { ModContent.ItemType<Bananarang>(), 10 },
            { ModContent.ItemType<LightDisc>(), 5 }
        };

        public static int[] FruitcakeChakramDebuffs { get => _fruitcakeChakramDebuffs; }

        public static int[] ItemsThatShootBoomerangs { get => _itemsThatShootBoomerangs; }

        public static Dictionary<int, int> BoomerangMaxOutCount { get => _boomerangMaxOutCount; }

        public override void Unload() {
            _fruitcakeChakramDebuffs = null;
            _itemsThatShootBoomerangs = null;
            _boomerangMaxOutCount = null;
        }

        public override void PostSetupContent() {
            for (int i = 0; i < ContentSamples.ItemsByType.Count; i++) {
                Item item = ContentSamples.ItemsByType[i];
                Projectile proj = ContentSamples.ProjectilesByType[item.shoot];
                if (proj.friendly && !proj.hostile && proj.aiStyle == 3 && proj.CountsAsClass(DamageClass.Melee)) {
                    if (!_boomerangMaxOutCount.ContainsKey(item.type)) {
                        _boomerangMaxOutCount.Add(item.type, -1);
                    }
                }
            }
        }
    }
}