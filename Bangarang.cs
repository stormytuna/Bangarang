using System;
using Bangarang.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace Bangarang;

public class Bangarang : Mod
{
    public override object Call(params object[] args) {
        if (args is null) {
            throw new ArgumentNullException(nameof(args), "Arguments cannot be null!");
        }

        if (args.Length == 0) {
            throw new ArgumentException("Arguments cannot be empty!");
        }

        if (args[0] is not int itemType) {
            throw new Exception($"Expected an argument of type int for args[0], but got type {args[0].GetType().Name} instead");
        }

        if (args[1] is not int[] and not int) {
            throw new Exception($"Expected an argument of type int or int[] for args[1], but got type {args[1].GetType().Name} instead");
        }

        if (args[2] is not int boomerangCount) {
            throw new Exception($"Expected an argument of type int for args[2], but got type {args[2].GetType().Name} instead");
        }

        if (args[3] is not Func<Player, Item, int, bool> and not null) {
            throw new Exception($"Expected an argument of type Func<Player, Item, int, bool> or null for args[3], but got type {args[3].GetType().Name} instead");
        }

        Func<Player, Item, int, bool> canUseItemFunc = args[3] as Func<Player, Item, int, bool>;
        if (args[1] is int projectileType) {
            BoomerangInfoSystem.RegisterBoomerang(itemType, projectileType, boomerangCount, canUseItemFunc);
        } else if (args[1] is int[] projectileTypes) {
            BoomerangInfoSystem.RegisterBoomerang(itemType, projectileTypes, boomerangCount, canUseItemFunc);
        }

        return true;
    }

    // TODO: Fix this
    /*
    public override void Load() {
        AddToggle("Mods.Bangarang.Config.ModdedBoomerangs", "Modded boomerangs", ModContent.ItemType<CONFIG_ChromaticCrux>(), "ffffff");
        AddToggle("Mods.Bangarang.Config.ModdedAccessories", "Modded accessories", ModContent.ItemType<CONFIG_Phylactery>(), "ffffff");
        AddToggle("Mods.Bangarang.Config.AutoSupport", "Automatically support other modded boomerangs", ModContent.ItemType<CONFIG_Cog>(), "ffffff");
    }

    private void AddToggle(string toggle, string name, int item, string color) {
        LocalizedText text = Language.GetOrRegister(toggle);
        // text.SetDefault($"[i:{item}] [c/{color}:{name}]");
        LocalizationLoader.AddTranslation(text); //tModPorter Note: Removed. Use Language.GetOrRegister
    }
    */
}