using System;
using System.Collections.Generic;
using System.Linq;
using Bangarang.Content.Items.Weapons;
using Bangarang.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Common.Systems;

public class BoomerangInfoSystem : ModSystem
{
    public class BoomerangInfo
    {
        public int[] ProjectileTypes { get; }
        public int NumBoomerangs { get; }
        public Func<Player, Item, int, bool> CanUseItemFunc { get; }

        public BoomerangInfo(int[] projectileTypes, int numBoomerangs, Func<Player, Item, int, bool> canUseItemFunc) {
            ProjectileTypes = projectileTypes;
            NumBoomerangs = numBoomerangs;
            CanUseItemFunc = canUseItemFunc;
        }
    }

    public static int[] ProjectilesThatAreBoomerangs { get; private set; } = Array.Empty<int>();

    public static Dictionary<int, BoomerangInfo> BoomerangInfoDict { get; private set; } = new();

    public override void Unload() {
        ProjectilesThatAreBoomerangs = null;
        BoomerangInfoDict = null;
    }

    public override void PostSetupContent() {
        // Adds vanilla boomerangs
        RegisterBoomerang(ItemID.WoodenBoomerang, ProjectileID.WoodenBoomerang, 1);
        RegisterBoomerang(ItemID.FruitcakeChakram, ProjectileID.FruitcakeChakram, 1);
        RegisterBoomerang(ItemID.BloodyMachete, ProjectileID.BloodyMachete, 1);
        RegisterBoomerang(ItemID.IceBoomerang, ProjectileID.IceBoomerang, 1);
        RegisterBoomerang(ItemID.EnchantedBoomerang, ProjectileID.EnchantedBoomerang, 1);
        RegisterBoomerang(ItemID.Trimarang, ProjectileID.Trimarang, 3);
        RegisterBoomerang(ItemID.Shroomerang, ProjectileID.Shroomerang, 1);
        RegisterBoomerang(ItemID.CombatWrench, ProjectileID.CombatWrench, 1);
        RegisterBoomerang(ItemID.ThornChakram, ProjectileID.ThornChakram, 1);
        RegisterBoomerang(ItemID.Flamarang, ProjectileID.Flamarang, 1);
        RegisterBoomerang(ItemID.FlyingKnife, ProjectileID.FlyingKnife, 1);
        RegisterBoomerang(ItemID.LightDisc, ProjectileID.LightDisc, 5);
        RegisterBoomerang(ItemID.PossessedHatchet, ProjectileID.PossessedHatchet, -1);
        RegisterBoomerang(ItemID.BouncingShield, ProjectileID.BouncingShield, 1);
        RegisterBoomerang(ItemID.PaladinsHammer, ProjectileID.PaladinsHammerFriendly, -1);
        RegisterBoomerang(ItemID.Bananarang, ProjectileID.Bananarang, 10);

        // Adds this mods boomerangs
        RegisterBoomerang(ModContent.ItemType<Beemerang>(), ModContent.ProjectileType<BeemerangProj>(), 1);
        RegisterBoomerang(ModContent.ItemType<Bonerang>(), ModContent.ProjectileType<BonerangProj>(), 1);
        RegisterBoomerang(ModContent.ItemType<ChromaticCrux>(), ModContent.ProjectileType<ChromaticCruxProj>(), 3);
        RegisterBoomerang(ModContent.ItemType<Rangaboom>(), ModContent.ProjectileType<RangaboomProj>(), 5);
        RegisterBoomerang(ModContent.ItemType<SawedOffShotrang>(), ModContent.ProjectileType<SawedOffShotrangProj>(), 2);
        RegisterBoomerang(ModContent.ItemType<ShadeChakram>(), ModContent.ProjectileType<ShadeChakramProj>(), 1);
        RegisterBoomerang(ModContent.ItemType<Synapse>(), ModContent.ProjectileType<SynapseProj>(), 1);
        RegisterBoomerang(ModContent.ItemType<Teslarang>(), ModContent.ProjectileType<TeslarangProj>(), 3);
        RegisterBoomerang(ModContent.ItemType<TheChloroplast>(), ModContent.ProjectileType<TheChloroplastProj>(), 3);
        RegisterBoomerang(ModContent.ItemType<WhiteDwarf>(), ModContent.ProjectileType<WhiteDwarfProj>(), 3);
        RegisterBoomerang(ModContent.ItemType<YinAndRang>(), ModContent.ProjectileType<YinAndRangProj>(), 2);
    }

    public static void RegisterBoomerang(int itemType, int projectileType, int numBoomerangs, Func<Player, Item, int, bool> canUseItemFunc = null) => RegisterBoomerang(itemType, new[] { projectileType }, numBoomerangs, canUseItemFunc);

    public static void RegisterBoomerang(int itemType, int[] projectileTypes, int numBoomerangs, Func<Player, Item, int, bool> canUseItemFunc = null) {
        // Not sure what was causing it, but someone reporting this method throwing
        if (BoomerangInfoDict.ContainsKey(itemType)) {
            ModContent.GetInstance<Bangarang>().Logger.Warn($"WARNING: Item with type {itemType} ({ItemID.Search.GetName(itemType)}) has already been registered!");
            return;
        }

        // Adds to our projectile list
        List<int> maxOutList = ProjectilesThatAreBoomerangs.ToList();
        maxOutList.AddRange(projectileTypes);
        ProjectilesThatAreBoomerangs = maxOutList.ToArray();

        // Adds to our dict
        BoomerangInfo bi = new(projectileTypes, numBoomerangs, canUseItemFunc);
        BoomerangInfoDict.Add(itemType, bi);
    }
}