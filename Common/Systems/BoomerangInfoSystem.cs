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

	public static int[] FruitcakeChakramDebuffs { get; private set; } = { BuffID.Confused, BuffID.CursedInferno, BuffID.Ichor, BuffID.Frostburn, BuffID.OnFire, BuffID.Poisoned, BuffID.ShadowFlame };

	public static int[] ProjectilesThatAreBoomerangs { get; private set; } = Array.Empty<int>();

	public static Dictionary<int, BoomerangInfo> BoomerangInfoDict { get; private set; } = new();

	public static int[] VeryRareItemIds { get; private set; } = { ItemID.BedazzledNectar, ItemID.ExoticEasternChewToy, ItemID.BirdieRattle, ItemID.AntiPortalBlock, ItemID.CompanionCube, ItemID.SittingDucksFishingRod, ItemID.HunterCloak, ItemID.WinterCape, ItemID.RedCape, ItemID.MysteriousCape, ItemID.CrimsonCloak, ItemID.DiamondRing, ItemID.CelestialMagnet, ItemID.WaterGun, ItemID.PulseBow, ItemID.YellowCounterweight };

	public override void Unload() {
		FruitcakeChakramDebuffs = null;
		ProjectilesThatAreBoomerangs = null;
		BoomerangInfoDict = null;
		VeryRareItemIds = null;
	}

	public override void PostSetupContent() {
		// Adds vanilla boomerangs
		RegisterBoomerang(ItemID.WoodenBoomerang, ProjectileID.WoodenBoomerang, 1, null);
		RegisterBoomerang(ItemID.FruitcakeChakram, ProjectileID.FruitcakeChakram, 1, null);
		RegisterBoomerang(ItemID.BloodyMachete, ProjectileID.BloodyMachete, 1, null);
		RegisterBoomerang(ItemID.IceBoomerang, ProjectileID.IceBoomerang, 1, null);
		RegisterBoomerang(ItemID.EnchantedBoomerang, ProjectileID.EnchantedBoomerang, 1, null);
		//RegisterBoomerang(ItemID.Trimarang, ProjectileID.Trimarang, 1, null); // TODO: Add Trimarang
		RegisterBoomerang(ItemID.Shroomerang, ProjectileID.Shroomerang, 1, null);
		RegisterBoomerang(ItemID.CombatWrench, ProjectileID.CombatWrench, 1, null);
		RegisterBoomerang(ItemID.ThornChakram, ProjectileID.ThornChakram, 1, null);
		RegisterBoomerang(ItemID.Flamarang, ProjectileID.Flamarang, 1, null);
		RegisterBoomerang(ItemID.FlyingKnife, ProjectileID.FlyingKnife, 1, null);
		RegisterBoomerang(ItemID.LightDisc, ProjectileID.LightDisc, 5, null);
		RegisterBoomerang(ItemID.PossessedHatchet, ProjectileID.PossessedHatchet, -1, null);
		RegisterBoomerang(ItemID.BouncingShield, ProjectileID.BouncingShield, 1, null);
		RegisterBoomerang(ItemID.PaladinsHammer, ProjectileID.PaladinsHammerFriendly, -1, null);
		RegisterBoomerang(ItemID.Bananarang, ProjectileID.Bananarang, 10, null);

		// Adds this mods boomerangs
		RegisterBoomerang(ModContent.ItemType<Bananarang>(), ProjectileID.Bananarang, 10, null);
		RegisterBoomerang(ModContent.ItemType<LightDisc>(), ProjectileID.LightDisc, 5, null);
		RegisterBoomerang(ModContent.ItemType<Beemerang>(), ModContent.ProjectileType<BeemerangProj>(), 1, null);
		RegisterBoomerang(ModContent.ItemType<Bonerang>(), ModContent.ProjectileType<BonerangProj>(), 1, null);
		RegisterBoomerang(ModContent.ItemType<ChromaticCrux>(), ModContent.ProjectileType<ChromaticCruxProj>(), 3, null);
		RegisterBoomerang(ModContent.ItemType<Rangaboom>(), ModContent.ProjectileType<RangaboomProj>(), 5, null);
		RegisterBoomerang(ModContent.ItemType<SawedOffShotrang>(), ModContent.ProjectileType<SawedOffShotrangProj>(), 2, null);
		RegisterBoomerang(ModContent.ItemType<ShadeChakram>(), ModContent.ProjectileType<ShadeChakramProj>(), 1, null);
		RegisterBoomerang(ModContent.ItemType<Synapse>(), ModContent.ProjectileType<SynapseProj>(), 1, null);
		RegisterBoomerang(ModContent.ItemType<Teslarang>(), ModContent.ProjectileType<TeslarangProj>(), 3, null);
		RegisterBoomerang(ModContent.ItemType<TheChloroplast>(), ModContent.ProjectileType<TheChloroplastProj>(), 3, null);
		RegisterBoomerang(ModContent.ItemType<WhiteDwarf>(), ModContent.ProjectileType<WhiteDwarfProj>(), 3, null);
		RegisterBoomerang(ModContent.ItemType<YinAndRang>(), ModContent.ProjectileType<YinAndRangProj>(), 2, null);
	}

	public static void RegisterBoomerang(int itemType, int projectileType, int numBoomerangs, Func<Player, Item, int, bool> canUseItemFunc) {
		RegisterBoomerang(itemType, new[] { projectileType }, numBoomerangs, canUseItemFunc);
	}

	public static void RegisterBoomerang(int itemType, int[] projectileTypes, int numBoomerangs, Func<Player, Item, int, bool> canUseItemFunc) {
		// Adds to our projectile list
		List<int> maxOutList = ProjectilesThatAreBoomerangs.ToList();
		maxOutList.AddRange(projectileTypes);
		ProjectilesThatAreBoomerangs = maxOutList.ToArray();

		// Adds to our dict
		BoomerangInfo bi = new(projectileTypes, numBoomerangs, canUseItemFunc);
		BoomerangInfoDict.Add(itemType, bi);
	}
}