using Bangarang.Common.Systems;
using Bangarang.Content.Items;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Bangarang {
    public class Bangarang : Mod {
        public override object Call(params object[] args) {
            if (args is null) {
                throw new ArgumentNullException(nameof(args), "Arguments cannot be null!");
            }

            if (args.Length == 0) {
                throw new ArgumentException("Arguments cannot be empty!");
            }

            if (args[0] is not int) {
                throw new Exception($"Expected an argument of type int for args[0], but got type {args[0].GetType().Name} instead");
            }
            if (args[1] is not int[] && args[1] is not int) {
                throw new Exception($"Expected an argument of type int or int[] for args[1], but got type {args[1].GetType().Name} instead");
            }
            if (args[2] is not int) {
                throw new Exception($"Expected an argument of type int for args[2], but got type {args[2].GetType().Name} instead");
            }
            if (args[3] is not Func<Player, Item, bool> && args[3] is not null) {
                throw new Exception($"Expected an argument of type Func<Player, Item, bool> or null for args[3], but got type {args[3].GetType().Name} instead");
            }

            int itemType = (int)args[0];
            int boomerangCount = (int)args[2];
            var canUseItemFunc = (Func<Player, Item, bool>)args[3];
            if (args[1] is int projectileType) {
                ArraySystem.RegisterBoomerang(itemType, projectileType, boomerangCount, canUseItemFunc);
            }
            else if (args[1] is int[] projectileTypes) {
                ArraySystem.RegisterBoomerang(itemType, projectileTypes, boomerangCount, canUseItemFunc);
            }
            return true;
        }

        public override void Load() {
            AddToggle("Mods.Bangarang.Config.ModdedBoomerangs", "Modded boomerangs", ModContent.ItemType<CONFIG_ChromaticCrux>(), "ffffff");
            AddToggle("Mods.Bangarang.Config.ModdedAccessories", "Modded accessories", ModContent.ItemType<CONFIG_Phylactery>(), "ffffff");
            AddToggle("Mods.Bangarang.Config.AutoSupport", "Automatically support other modded boomerangs", ModContent.ItemType<CONFIG_Cog>(), "ffffff");
        }

        private void AddToggle(string toggle, string name, int item, string color) {
            ModTranslation text = LocalizationLoader.CreateTranslation(toggle);
            text.SetDefault($"[i:{item}] [c/{color}:{name}]");
            LocalizationLoader.AddTranslation(text);
        }
    }
}