using HarmonyLib;
using Timberborn.BlockingSystem;
using Timberborn.Buildings;
using Timberborn.InventoryNeedSystem;
using UnityEngine;

namespace Calloatti.PausableStorage
{
  [HarmonyPatch]
  public static class ModPatchesNeeds
  {
    /// <summary>
    /// Intercepts beavers trying to fulfill personal needs (thirst, hunger) directly.
    /// If the building is paused or blocked, we return a null ActionPosition,
    /// causing the logic tree to treat the building as an invalid destination.
    /// </summary>
    [HarmonyPatch(typeof(InventoryNeedBehavior), nameof(InventoryNeedBehavior.ActionPosition))]
    [HarmonyPrefix]
    public static bool ActionPosition_Prefix(InventoryNeedBehavior __instance, ref Vector3? __result)
    {
      // ONLY apply custom blocking logic if it has our specific storage component
      if (__instance.GetComponent<PausableStorageComponent>() != null)
      {
        // 1. Manual pause check
        var pausable = __instance.GetComponent<PausableBuilding>();
        if (pausable != null && pausable.Paused)
        {
          __result = null;
          return false; // Skip original method
        }

        // 2. Automation block check
        var blockable = __instance.GetComponent<BlockableObject>();
        if (blockable != null && !blockable.IsUnblocked)
        {
          __result = null;
          return false; // Skip original method
        }
      }

      return true; // Proceed with original method
    }
  }
}