using HarmonyLib;
using Timberborn.Modding;
using Timberborn.ModManagerScene;

namespace Calloatti.PausableStorage
{
  public class PausableStorageModStarter : IModStarter
  {
    public void StartMod(IModEnvironment modEnvironment)
    {
      // Re-enable Harmony to inject our custom input/output logic rules
      var harmony = new Harmony("calloatti.pausablestorage");
      harmony.PatchAll();
    }
  }
}