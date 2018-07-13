﻿namespace MoreCyclopsUpgrades
{
    using System;
    using System.IO;
    using System.Reflection;
    using Harmony;
    using SMLHelper.V2.Handlers;

    public class QPatch
    {
        private static SortedCyclopsModules ModulesToPatch;
        private static NuclearModuleConfig Config;

        public static void Patch()
        {
#if DEBUG
            try
            {

#endif
                bool hasVehicleUpgradesInCyclops = Directory.Exists(@"./QMods/VehicleUpgradesInCyclops");

                ModulesToPatch = new SortedCyclopsModules(7)
                {
                    new SolarCharger(hasVehicleUpgradesInCyclops),
                    new SolarChargerMk2(),
                    new ThermalChargerMk2(),
                    new PowerUpgradeMk2(),
                    new PowerUpgradeMk3(),
                    new NuclearCharger(),
                    new DepletedNuclearModule(),
                };

                foreach (CyclopsModule module in ModulesToPatch.Values)
                {
                    module.Patch();
                }

                NuclearFabricator.Patch();
                
                Config = new NuclearModuleConfig();
                OptionsPanelHandler.RegisterModOptions(Config);
                Config.Initialize();

                HarmonyInstance harmony = HarmonyInstance.Create("com.morecyclopsupgrades.psmod");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
#if DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine("[MoreCyclopsUpgrades] ERROR: " + ex.ToString());
            }
#endif
        }
    }
}