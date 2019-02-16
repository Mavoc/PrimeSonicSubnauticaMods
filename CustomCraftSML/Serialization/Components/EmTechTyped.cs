﻿namespace CustomCraft2SML.Serialization.Components
{
    using System.Collections.Generic;
    using Common;
    using Common.EasyMarkup;
    using CustomCraft2SML.Interfaces;
    using CustomCraft2SML.PublicAPI;
    using SMLHelper.V2.Handlers;

    public abstract class EmTechTyped : EmPropertyCollection, ITechTyped
    {
        protected readonly EmProperty<string> emTechType;

        protected static List<EmProperty> TechTypedProperties => new List<EmProperty>(1)
        {
            new EmProperty<string>("ItemID"),
        };

        public EmTechTyped() : this("TechTyped", TechTypedProperties)
        {
        }

        protected EmTechTyped(string key) : this(key, TechTypedProperties)
        {
        }

        protected EmTechTyped(string key, ICollection<EmProperty> definitions) : base(key, definitions)
        {
            emTechType = (EmProperty<string>)Properties["ItemID"];
        }

        public string ItemID
        {
            get => emTechType.Value;
            set => emTechType.Value = value;
        }

        public TechType TechType { get; set; } = TechType.None;

        public virtual bool PassesPreValidation()
        {
            // Now we can safely do the prepass check in case we need to create a new modded TechType
            this.TechType = GetTechType(this.ItemID);

            if (TechType == TechType.None)
            {
                QuickLogger.Warning($"Could not resolve ID of '{this.ItemID}'. Discarded entry.");
                return false;
            }

            return true;
        }

        protected static TechType GetTechType(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return TechType.None;
            }

            // Look for a known TechType
            if (TechTypeExtensions.FromString(value, out TechType tType, true))
            {
                return tType;
            }

            //  Not one of the known TechTypes - is it registered with SMLHelper?
            if (TechTypeHandler.TryGetModdedTechType(value, out TechType custom))
            {
                return custom;
            }

            return TechType.None;
        }

        protected static void AddCraftNode(CraftingPath newPath, TechType itemID)
        {
            if (newPath.IsAtRoot)
            {
                CraftTreeHandler.AddCraftingNode(newPath.Scheme, itemID);
                QuickLogger.Message($"New crafting node for '{itemID}' added to the root of the {newPath.Scheme} crafting tree");
            }
            else
            {
                CraftTreeHandler.AddCraftingNode(newPath.Scheme, itemID, newPath.Steps);
                QuickLogger.Message($"New crafting node for '{itemID}' added to the {newPath.Scheme} crafting tree at {newPath.Path}");
            }
        }
    }
}
