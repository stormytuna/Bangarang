using Bangarang.Common.Configs;
using Bangarang.Content.Items.Weapons;
using Bangarang.Content.Projectiles.Weapons;
using System.Collections.Generic;
using System.Linq;
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

        private static int[] _projectilesThatAreBoomerangs = new int[] {
            ProjectileID.WoodenBoomerang,
            ProjectileID.EnchantedBoomerang,
            ProjectileID.FruitcakeChakram,
            ProjectileID.BloodyMachete,
            ProjectileID.Shroomerang,
            ProjectileID.IceBoomerang,
            ProjectileID.ThornChakram,
            ProjectileID.CombatWrench,
            ProjectileID.Flamarang,
            ProjectileID.Bananarang,
            ProjectileID.FlyingKnife,
            ProjectileID.BouncingShield,
            ProjectileID.LightDisc,
            ProjectileID.PaladinsHammerFriendly,
            ProjectileID.PossessedHatchet,
            ModContent.ProjectileType<BeemerangProj>(),
            ModContent.ProjectileType<BonerangProj>(),
            ModContent.ProjectileType<ChromaticCruxProj>(),
            ModContent.ProjectileType<RangaboomProj>(),
            ModContent.ProjectileType<SawedOffShotrangProj>(),
            ModContent.ProjectileType<ShadeChakramProj>(),
            ModContent.ProjectileType<SynapseProj>(),
            ModContent.ProjectileType<TeslarangProj>(),
            ModContent.ProjectileType<TheChloroplastProj>(),
            ModContent.ProjectileType<WhiteDwarfProj>(),
            ModContent.ProjectileType<YinAndRangProj>()
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
            { ItemID.PossessedHatchet, -1 },
            { ModContent.ItemType<Bananarang>(), 10 },
            { ModContent.ItemType<LightDisc>(), 5 },
            { ModContent.ItemType<Beemerang>(), -1 },
            { ModContent.ItemType<Bonerang>(), -1 },
            { ModContent.ItemType<ChromaticCrux>(), -1 },
            { ModContent.ItemType<Rangaboom>(), -1 },
            { ModContent.ItemType<SawedOffShotrang>(), -1 },
            { ModContent.ItemType<ShadeChakram>(), -1 },
            { ModContent.ItemType<Synapse>(), -1 },
            { ModContent.ItemType<Teslarang>(), -1 },
            { ModContent.ItemType<TheChloroplast>(), -1 },
            { ModContent.ItemType<WhiteDwarf>(), -1 },
            { ModContent.ItemType<YinAndRang>(), -1 }
        };

        public static int[] FruitcakeChakramDebuffs { get => _fruitcakeChakramDebuffs; }

        public static int[] ProjectilesThatAreBoomerangs { get => _projectilesThatAreBoomerangs; }

        public static Dictionary<int, int> BoomerangMaxOutCount { get => _boomerangMaxOutCount; }

        public override void Unload() {
            _fruitcakeChakramDebuffs = null;
            _projectilesThatAreBoomerangs = null;
            _boomerangMaxOutCount = null;
        }

        public override void PostSetupContent() {
            if (ServerConfig.Instance.AssumeModdedBoomerangs) {
                List<int> temp = _projectilesThatAreBoomerangs.ToList();
                foreach (var proj in ContentSamples.ProjectilesByType.Values) {
                    if (proj.aiStyle == 3 && !_projectilesThatAreBoomerangs.Contains(proj.type)) {
                        temp.Add(proj.type);
                    }
                }
                _projectilesThatAreBoomerangs = temp.ToArray();
            }
        }

        public static void RegisterBoomerang(int type, int numBoomerangs) {
            if (!_boomerangMaxOutCount.ContainsKey(type)) {
                _boomerangMaxOutCount.Add(type, numBoomerangs);
            }
        }
    }
}