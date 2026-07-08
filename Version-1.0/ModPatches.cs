using System;
using HarmonyLib;
using Timberborn.Buildings;
using Timberborn.InventorySystem;
using Timberborn.Navigation;
using Timberborn.BlockingSystem; // Added to check automation block states
using UnityEngine;

namespace Calloatti.PausableStorage
{
  [HarmonyPatch]
  public static class ModPatches
  {
    /// <summary>
    /// Intercepts the primary pathfinding method by wrapping the vanilla Predicate parameter.
    /// This entirely avoids duplicating the vanilla loop or pathfinding logic!
    /// </summary>
    [HarmonyPatch(typeof(DistrictInventoryPicker), nameof(DistrictInventoryPicker.ClosestInventoryWithStock), new Type[] { typeof(Accessible), typeof(string), typeof(Predicate<Inventory>) })]
    [HarmonyPrefix]
    public static void ClosestInventoryWithStock_Prefix1(ref Predicate<Inventory> inventoryFilter)
    {
      // Save a reference to whatever filter the vanilla game is trying to use
      var originalFilter = inventoryFilter;

      // Inject our wrapper
      inventoryFilter = inventory =>
      {
        // ONLY apply custom blocking logic if it has our specific storage component
        if (inventory.GetComponent<PausableStorageComponent>() != null)
        {
          // 1. Check if manually paused by the player
          var pausable = inventory.GetComponent<PausableBuilding>();
          if (pausable != null && pausable.Paused) return false;

          // 2. Check if blocked by automation/logic networks
          var blockable = inventory.GetComponent<BlockableObject>();
          if (blockable != null && !blockable.IsUnblocked) return false;
        }

        return originalFilter == null || originalFilter(inventory); // Otherwise, evaluate vanilla filter
      };

      // We do NOT return false here. The vanilla method runs natively using our injected filter!
    }

    /// <summary>
    /// The secondary overload does NOT accept a Predicate parameter. 
    /// To avoid an extremely brittle IL Transpiler, we reluctantly duplicate the loop here.
    /// </summary>
    [HarmonyPatch(typeof(DistrictInventoryPicker), nameof(DistrictInventoryPicker.ClosestInventoryWithStock), new Type[] { typeof(Vector3), typeof(string), typeof(Accessible) })]
    [HarmonyPrefix]
    public static bool ClosestInventoryWithStock_Prefix2(DistrictInventoryPicker __instance, Vector3 start, string goodId, Accessible accessibleReachableFromInventory, ref Inventory __result)
    {
      var registry = __instance.GetComponent<DistrictInventoryRegistry>();
      var readOnlyHashSet = registry.ActiveInventoriesWithStock(goodId);

      Inventory result = null;
      float num = float.MaxValue;

      foreach (Inventory item in readOnlyHashSet)
      {
        // ONLY apply custom blocking logic if it has our specific storage component
        if (item.GetComponent<PausableStorageComponent>() != null)
        {
          // 1. Manual pause check
          var pausable = item.GetComponent<PausableBuilding>();
          if (pausable != null && pausable.Paused) continue;

          // 2. Automation block check
          var blockable = item.GetComponent<BlockableObject>();
          if (blockable != null && !blockable.IsUnblocked) continue;
        }

        Accessible enabledComponent = item.GetEnabledComponent<Accessible>();
        if (enabledComponent.IsReachableByRoad(accessibleReachableFromInventory) && enabledComponent.FindRoadPath(start, out var distance) && distance < num)
        {
          result = item;
          num = distance;
        }
      }

      __result = result;
      return false;
    }
  }
}