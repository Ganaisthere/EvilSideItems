using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace EvilSideItems
{
    [HarmonyPatch(typeof(SodaMachine))]
    public class SodaMachinePatches
    {
        public static Dictionary<string, Material> SavedMaterials = new Dictionary<string, Material>();

        [HarmonyPatch("InsertItem")]
        [HarmonyPrefix]
        public static void InsertItemPrefix(SodaMachine __instance)
        {
            Material material = __instance.GetComponent<MeshRenderer>().sharedMaterials[1];
            if (!material.name.Contains("_Out") && !SavedMaterials.ContainsKey(__instance.name))
            {
                SavedMaterials.Add(__instance.name, material);
            }
        }
    }
}
