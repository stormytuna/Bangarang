using Bangarang.Common.Systems;
using System;
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

            if (args[0] is int itemType) {
                if (args[1] is int numBoomerangs) {
                    ArraySystem.RegisterBoomerang(itemType, numBoomerangs);
                    return true;
                }
                else {
                    throw new Exception($"Expected an argument of type int, but got type {args[0].GetType().Name} instead");
                }
            }
            else {
                throw new Exception($"Expected an argument of type int, but got type {args[0].GetType().Name} instead");
            }
        }
    }
}