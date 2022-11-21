# Bangarang

Bangarang is a mod that overhauls the Boomerang subclass of melee weapons, making them more diverse, giving them unique effects and adding an array of new boomerangs

Vanilla changes are pretty small in this mod, giving some boomerangs minor effects, giving a couple boomerangs crafting recipes and allowing stacking boomerangs to receive modifiers

New boomerangs are available throughout your entire progression. From the Shade Chakram, an early pre-hardmode boomerang that slightly homes on enemies, to the White Dwarf, an endgame boomerang that incinerates all it touches. This mod also adds a handful of accessories that support Boomerangs, increasing how many you can throw, how fast they return and more

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
* Shade Chakram - Travels exceptionally quick. Crafted with 8 Demonite Bars, 5 Shadow Scales @ Anvil
* Beemerang - Releases bees on impact. Crafted with 14 Bee Wax @ Anvil
* Bonerang - Breaks into bone shards on impact. Crafted with 90 Bones @ Anvil
* Sawed Off Shotrang - Fires a shotgun blast at its apex. Crafted with 1 Shotgun, 1 Illegal Gun Parts @ Anvil
* Yin and Rang - Breaks into two homing shards at its apex. Crafted with 1 Light Shard, 1 Dark Shard, 5 Titanium Bars @ Hardmode Anvil
* The Chloroplast - Bursts into stingers at its apex. Crafted with 14 Chlorophyte Bars @ Hardmode Anvil
* Teslarang - Discharges bolts of electricity when it hits an enemy. 2.5% drop from Frankenstein after Plantera has been defeated
* Chromatic Crux - Leaves a homing rainbow chakram when it hits an enemy. Drops from Empress of Light
* Rangaboom - Starts at its apex and travels to you. Drops from Martian Saucer
* White Dwarf - Incinerates enemies with stellar energy. Crafted with 16 Solar Fragments @ Ancient Manipulator

## **Added accessories**

* Boomerang License - Lets you throw an extra boomerang. Bought from the Travelling Merchant after any boss has been defeated
* Illuminant Coating - Your boomerangs glow, return faster and have increased knockback. 2% drop from Illuminant Bat and Slime
* Spectral Amulet - Your boomerangs are orbited by a pair of spectral glaives. 1.25% drop from any magic Post-Plantera Dungeon skeleton
* Fluorescent License - Effects of Boomerang License and Illuminant Coating. Crafted with both of them @ Tinkerer's Workbench
* Ranger's Talisman - Effects of all 3. Crafted with Fluorescent License and Spectral Amulet @ Tinkerer's Workbench

***

## **For Modders**

<details>
<summary>Mod Call template</summary>
<br>


        // You can use this mod call to register your boomerangs to this mod's list of boomerangs
        // This allows accessories from this mod to work with your boomerangs
        // You can see how your mod call is interpreted and used here - https://github.com/stormytuna/Bangarang/blob/main/Bangarang.cs
        public override void PostSetupContent() {
            if (ModLoader.TryGetMod("Bangarang", out Mod bangarang)) {
                bangarang.Call(arg1, arg2, arg3, arg4);
                // arg1 should be an int equal to the item type of the boomerang you want to register
                // arg2 can be an int equal to the projectile type that the item shoots
                // arg2 can also be an int[] of all the projectile types that item shoots
                    // When checking if you can or can't use an item, it will use the highest count of any of the given projectiles, not the total count of all of those projectiles
                // arg3 should be an int equal to the number of boomerangs you can have out at once
                    // This allows the Boomerang License and its upgrades to allow your boomerang to be thrown an extra time
                    // Use the base amount of boomerangs, if you can throw 5 you should be able to throw 6 with the Boomerang License
                    // Use -1 if this boomerang doesn't have a limit
                    // Use -2 if this boomerang shouldn't benefit from the extra boomerang effect of Boomerang License and its upgrades
                // arg4 can be a Func<Player, Item, int, bool>, ie a function that takes a Player, Item and int as parameters and returns a bool
                    // The Player will be the player currently trying to use the item
                    // The Item will be the item they're trying to use
                    // The int will be the number of extra boomerangs, currently this will only be 0 or 1 but assuming it could be greater than 1 when writing code using it
                    // If your boomerang is prevented from shooting based on anything except count you will want to use this
                    // If your boomerang is limited by count, don't add that condition to this Func
                    // This is also called if arg3 is -1 or -2
                // arg4 can also be null if you don't need to add this sort of extra functionality
                    // However it's important to actually pass in null here, not doing so results in an index out of range exception 
            }
        }



</details>

***

To report any bugs or give feedback, you can post a comment on the Steam Workshop or message me on Discord (stormytuna#2688). You can report bugs using GitHub issues as well.