using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace GorillaTagModTemplateProject.Patches
{
    /// <summary>
    /// This is an example patch, made to demonstrate how to use Harmony. You should remove it if it is not used.
    /// </summary>
    [HarmonyPatch(typeof(VRRig), "PlayHandTapLocal"), HarmonyWrapSafe]
    class HandTapPatch
    {
        public static int stepsCount;
        static void Postfix(VRRig __instance)
        {
            if (__instance.isOfflineVRRig)
            {
                stepsCount++;
            }
        }
    }
}
