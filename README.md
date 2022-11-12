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

Alrighty, this mod call takes 3 parameters that are all integers. First arg should be the item type of your boomerang, the second arg should be the projectile type of your boomerang, the third arg should be the maximum number of that boomerang the player can shoot


        public override void PostSetupContent() {
            if (ModLoader.TryGetMod("Bangarang", out Mod bangarang)) {
                bangarang.Call(ModContent.ItemType<X>(), ModContent.ProjectileType<Y>(), Z);
                // X is your item class
                // Y is your projectile class
                // Z is the amount of boomerangs the player can throw
            }
        }



</details>

***

To report any bugs or give feedback, you can post a comment on the Steam Workshop or message me on Discord (stormytuna#2688). You can report bugs using GitHub issues as well.