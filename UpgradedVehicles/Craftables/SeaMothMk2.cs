﻿namespace UpgradedVehicles
{
    using System.Collections.Generic;
    using SMLHelper.V2.Crafting;

    internal class SeaMothMk2 : UpgradedVehicle<SeaMoth>
    {
        internal SeaMothMk2(VehiclePowerCore vehiclePowerCore)
            : base(nameID: "SeaMothMk2",
                      friendlyName: "Seamoth MK2",
                      description: "An upgraded SeaMoth, built harder and faster to take you anywhere.",
                      template: TechType.Seamoth,
                      healthModifier: 2f, // 2x the Max HP. 100% more.
                      requiredAnalysis: TechType.VehicleHullModule3,
                      powerCore: vehiclePowerCore)
        {
        }

        protected override void PostPatch()
        {
            MTechType.SeaMothMk2 = this.TechType;
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[5]
                             {
                                 new Ingredient(TechType.PlasteelIngot, 1), // Stronger than titanium ingot
                                 new Ingredient(TechType.EnameledGlass, 2), // Stronger than glass
                                 new Ingredient(TechType.Lead, 1),

                                 new Ingredient(TechType.VehicleHullModule3, 1), // Minimum crush depth of 900 without upgrades
                                 new Ingredient(PowerCore.TechType, 1), // armor and speed without engine efficiency penalty
                             })
            };
        }
    }
}
