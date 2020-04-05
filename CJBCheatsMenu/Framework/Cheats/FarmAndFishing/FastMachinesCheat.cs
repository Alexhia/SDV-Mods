using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using SObject = StardewValley.Object;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing
{
    /// <summary>A cheat which makes machines complete their output instantly.</summary>
    internal class FastMachinesCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            return this.SortFields(
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.bee-house"),
                    value: context.Config.FastBeeHouse,
                    setValue: value => context.Config.FastBeeHouse = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.cask"),
                    value: context.Config.FastCask,
                    setValue: value => context.Config.FastCask = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.charcoal-kiln"),
                    value: context.Config.FastCharcoalKiln,
                    setValue: value => context.Config.FastCharcoalKiln = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.cheese-press"),
                    value: context.Config.FastCheesePress,
                    setValue: value => context.Config.FastCheesePress = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.crystalarium"),
                    value: context.Config.FastCrystalarium,
                    setValue: value => context.Config.FastCrystalarium = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.fruit-trees"),
                    value: context.Config.FastFruitTree,
                    setValue: value => context.Config.FastFruitTree = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.furnace"),
                    value: context.Config.FastFurnace,
                    setValue: value => context.Config.FastFurnace = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.incubator"),
                    value: context.Config.FastIncubator,
                    setValue: value => context.Config.FastIncubator = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.keg"),
                    value: context.Config.FastKeg,
                    setValue: value => context.Config.FastKeg = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.lightning-rod"),
                    value: context.Config.FastLightningRod,
                    setValue: value => context.Config.FastLightningRod = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.loom"),
                    value: context.Config.FastLoom,
                    setValue: value => context.Config.FastLoom = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.mayonnaise-machine"),
                    value: context.Config.FastMayonnaiseMachine,
                    setValue: value => context.Config.FastMayonnaiseMachine = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.mushroom-box"),
                    value: context.Config.FastMushroomBox,
                    setValue: value => context.Config.FastMushroomBox = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.oil-maker"),
                    value: context.Config.FastOilMaker,
                    setValue: value => context.Config.FastOilMaker = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.preserves-jar"),
                    value: context.Config.FastPreservesJar,
                    setValue: value => context.Config.FastPreservesJar = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.recycling-machine"),
                    value: context.Config.FastRecyclingMachine,
                    setValue: value => context.Config.FastRecyclingMachine = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.seed-maker"),
                    value: context.Config.FastSeedMaker,
                    setValue: value => context.Config.FastSeedMaker = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.slime-egg-press"),
                    value: context.Config.FastSlimeEggPress,
                    setValue: value => context.Config.FastSlimeEggPress = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.slime-incubator"),
                    value: context.Config.FastSlimeIncubator,
                    setValue: value => context.Config.FastSlimeIncubator = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.tapper"),
                    value: context.Config.FastTapper,
                    setValue: value => context.Config.FastTapper = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.wood-chipper"),
                    value: context.Config.FastWoodChipper,
                    setValue: value => context.Config.FastWoodChipper = value
                ),
                new CheatsOptionsCheckbox(
                    label: context.Text.Get("fast-machines.worm-bin"),
                    value: context.Config.FastWormBin,
                    setValue: value => context.Config.FastWormBin = value
                )
            );
        }

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = this
                .GetFields(context)
                .Cast<CheatsOptionsCheckbox>()
                .Any(p => p.IsChecked);
            needsRendering = false;
        }

        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!e.IsOneSecond || !Context.IsWorldReady)
                return;

            foreach (GameLocation location in context.GetAllLocations())
            {
                foreach (SObject obj in location.objects.Values)
                {
                    if (this.IsFastMachine(context, obj))
                        this.CompleteMachine(location, obj);
                }

                if (context.Config.FastFruitTree)
                {
                    foreach (TerrainFeature terrainFeature in location.terrainFeatures.Values)
                    {
                        if (terrainFeature is FruitTree tree && tree.growthStage.Value >= FruitTree.treeStage && tree.fruitsOnTree.Value < FruitTree.maxFruitsOnTrees)
                            tree.fruitsOnTree.Value = FruitTree.maxFruitsOnTrees;
                    }
                }
            }
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get whether an object is a machine with 'fast processing' enabled.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="obj">The machine to check.</param>
        private bool IsFastMachine(CheatContext context, SObject obj)
        {
            if (obj == null || !obj.bigCraftable.Value)
                return false;

            ModConfig config = context.Config;
            return
                config.FastBeeHouse && obj.name == "Bee House"
                || config.FastCask && obj is Cask
                || config.FastCharcoalKiln && obj.name == "Charcoal Kiln"
                || config.FastCheesePress && obj.name == "Cheese Press"
                || config.FastCrystalarium && obj.name == "Crystalarium"
                || config.FastFurnace && obj.name == "Furnace"
                || config.FastIncubator && obj.name == "Incubator"
                || config.FastKeg && obj.name == "Keg"
                || config.FastLightningRod && obj.name == "Lightning Rod"
                || config.FastLoom && obj.name == "Loom"
                || config.FastMayonnaiseMachine && obj.name == "Mayonnaise Machine"
                || config.FastMushroomBox && obj.name == "Mushroom Box"
                || config.FastOilMaker && obj.name == "Oil Maker"
                || config.FastPreservesJar && obj.name == "Preserves Jar"
                || config.FastRecyclingMachine && obj.name == "Recycling Machine"
                || config.FastSeedMaker && obj.name == "Seed Maker"
                || config.FastSlimeEggPress && obj.name == "Slime Egg-Press"
                || config.FastSlimeIncubator && obj.name == "Slime Incubator"
                || config.FastTapper && obj.name == "Tapper"
                || config.FastWoodChipper && obj is WoodChipper
                || config.FastWormBin && obj.name == "Worm Bin";
        }

        /// <summary>Finish a machine's processing.</summary>
        /// <param name="location">The machine's location.</param>
        /// <param name="machine">The machine to complete.</param>
        private void CompleteMachine(GameLocation location, SObject machine)
        {
            if (machine.heldObject.Value == null)
                return;

            // egg incubator
            // (animalHouse.incubatingEgg.X is the number of days until the egg hatches; Y is the egg ID.)
            if (location is AnimalHouse animalHouse && machine.bigCraftable.Value && machine.ParentSheetIndex == 101 && animalHouse.incubatingEgg.X > 0)
                animalHouse.incubatingEgg.X = 1;

            // other machines
            else if (machine.MinutesUntilReady > 0)
            {
                if (machine is Cask cask)
                {
                    cask.daysToMature.Value = 0;
                    cask.checkForMaturity();
                }
                machine.minutesElapsed(machine.MinutesUntilReady, location);
            }
        }
    }
}