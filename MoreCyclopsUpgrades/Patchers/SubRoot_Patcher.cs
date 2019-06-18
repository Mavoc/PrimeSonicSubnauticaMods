﻿namespace MoreCyclopsUpgrades.Patchers
{
    using Harmony;
    using Managers;
    using MoreCyclopsUpgrades.API;

    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch("UpdateThermalReactorCharge")]
    internal class SubRoot_UpdateThermalReactorCharge_Patcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ref SubRoot __instance)
        {
            CyclopsManager.GetManager(__instance)?.QuickChargeManager?.RechargeCyclops();

            // No need to execute original method anymore
            return false; // Completely override the method and do not continue with original execution
        }
    }

    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch("UpdatePowerRating")]
    internal class SubRoot_UpdatePowerRating_Patcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ref SubRoot __instance)
        {
            return false; // Now handled by UpgradeManager HandleUpgrades
        }
    }

    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch("SetCyclopsUpgrades")]
    internal class SubRoot_SetCyclopsUpgrades_Patcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ref SubRoot __instance)
        {
            LiveMixin cyclopsLife = __instance.live;

            if (cyclopsLife == null || !cyclopsLife.IsAlive())
                return true; // safety check

            CyclopsManager.GetManager<UpgradeManager>(__instance, UpgradeManager.ManagerName)?.HandleUpgrades();

            // No need to execute original method anymore
            return false; // Completely override the method and do not continue with original execution
        }
    }

    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch("SetExtraDepth")]
    internal class SubRoot_SetExtraDepth_Patcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ref SubRoot __instance)
        {
            return false; // Now handled by UpgradeManager HandleUpgrades
        }
    }

    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch("OnPlayerEntered")]
    internal class SubRoot_OnPlayerEntered_BeQuiet
    {
        private static bool firstEventDone = false;
        private static VoiceNotificationManager reference;

        [HarmonyPrefix]
        public static void Prefix(ref SubRoot __instance)
        {
            if (firstEventDone)
                return;

            if (reference != null || __instance.voiceNotificationManager == null)
                return;

            reference = __instance.voiceNotificationManager;
            __instance.voiceNotificationManager = null;
        }

        [HarmonyPostfix]
        public static void Postfix(ref SubRoot __instance)
        {
            if (reference != null && __instance.voiceNotificationManager == null)
            {
                __instance.voiceNotificationManager = reference;
            }

            firstEventDone = true;
        }
    }
}
