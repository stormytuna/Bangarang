# Bangarang

Bangarang is a mod that overhauls the Boomerang subclass of melee weapons, making them more diverse, giving them unique effects and adding an array of new boomerangs

Vanilla changes are pretty small in this mod, giving some boomerangs minor effects, giving a couple boomerangs crafting recipes and allowing stacking boomerangs to receive modifiers

New boomerangs are available throughout your entire progression. From the Shade Chakram, an early hardmode boomerang that returns faster, to the White Dwarf, an endgame boomerang that incinerates all it touches. This mod also adds a handful of accessories that support Boomerangs, increasing how many you can throw, how fast they return and more

***

## **Vanilla changes**

* All boomerangs have auto-reuse
* Bananarang and Light Disc now only stack to one. Light Disc has a more expensive recipe to compensate
* Bananarang has an increased drop chance, 16.67% from 3.33%
* Fruitcake Chakram now inflicts a random debuff on hit and can be acquired by shaking Boreal trees
* Bloody Machete now inflicts up to 50% more damage the longer it has been in flight
* Shroomerang now leaves mushrooms in the air as it flies
* Ice Boomerang now fires a couple icey crystals when it hits an enemy
* Combat Wrench now deals 20% more damage when it's returning
* Flamarang now fires a couple sparks when it hits an enemy

## **Added boomerangs**

* Synapse - Homes in on enemies. Crafted with 8 Crimtane Bars, 5 Tissue Samples @ Anvil
* Shade Chakram - Returns quickly. Crafted with 8 Demonite Bars, 5 Shadow Scales @ Anvil
* Beemerang - Releases bees on impact. Crafted with 14 Bee Wax @ Anvil
* Bonerang - Breaks into bone shards on impact. Crafted with 30 Bones @ Anvil
* Sawed Off Shotrang - Fires a shotgun blast at its apex. Crafted with 1 Shotgun, 1 Illegal Gun Parts @ Anvil
* Yin and Rang - Breaks into two homing shards at its apex. Crafted with 1 Light Shard, 1 Dark Shard, 5 Titanium Bars @ Hardmode Anvil
* The Chloroplast - Bursts into stingers at its apex. Crafted with 14 Chlorophyte Bars @ Hardmode Anvil
* Teslarang - Discharges bolts of electricity when it hits an enemy. 2.5% drop from Frankenstein after Plantera has been defeated
* Chromatic Crux - Leaves a homing rainbow chakram when it hits an enemy. Drops from Empress of Light
* Rangaboom - Starts at its apex and travels to you. Drops from Martian Saucer
* White Dwarf - Incinerates enemies with stellar energy. Crafted with 12 Solar Fragments, 8 Luminite Bars @ Ancient Manipulator

## **Added accessories**

* Boomerang License - Lets you throw an extra boomerang. Bought from the Travelling Merchant after any boss has been defeated
* Illuminant Coating - Your boomerangs glow, return faster and have increased knockback. 2% drop from Illuminant Bat and Slime
* Spectral Amulet - Your boomerangs are orbited by a pair of spectral glaives. 1.25% drop from any magic Post-Plantera Dungeon skeleton
* Fluorescent License - Effects of Boomerang License and Illuminant Coating. Crafted with both of them @ Tinkerer's Workbench
* Phylactery - Effects of all 3. Crafted with Fluorescent License and Spectral Amulet @ Tinkerer's Workbench

***

## **For Modders**

<details>
<summary>Mod Call template</summary>
<br>
This mod only has one mod call, it expects 2 args, both ints. It expects the first int to be the type of your boomerang projectile, while the second int is how many of those boomerangs you're allowed out at a time. Here's a template that you can throw in a <code>PostSetupContent</code> override


        public override void PostSetupContent() {
            if (ModLoader.TryGetMod("Bangarang", out Mod bangarang)) {
                bangarang.Call(ModContent.ProjectileType<X>(), Y);
                // X is the name of your boomerang projectile
                // Y is the maximum amount of boomerangs the player should be able to throw
            }
        }

<details>
<summary>If you care how your code here is used, open this!</summary>
<br>

Okay so, [here](https://github.com/stormytuna/Bangarang/blob/main/Bangarang.cs#L8) is where your call will be parsed. It then calls [this](https://github.com/stormytuna/Bangarang/blob/main/Common/Systems/ArraySystem.cs) function, registering your projectile type to an internal dict and array. 
<br>
The dict is used by [this](https://github.com/stormytuna/Bangarang/blob/main/Common/Players/DetourPlayer.cs#L14) detour, that just lets the player throw extra boomerangs
<br>
The array is used by [this](https://github.com/stormytuna/Bangarang/blob/main/Common/GlobalProjectiles/BangarangGlobalProjectile.cs#L15) global projectile override and [this](https://github.com/stormytuna/Bangarang/blob/main/Common/GlobalItems/BangarangGlobalItem.cs#L11) global item override, the former lets projetiles be recognised as boomerangs and the latter just enables auto-reuse

</details>
</details>

***

To report any bugs or give feedback, you can post a comment on the Steam Workshop or message me on Discord (stormytuna#2688). You can report bugs using GitHub issues as well.