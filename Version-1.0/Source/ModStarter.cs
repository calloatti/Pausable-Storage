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
new Harmony("Calloatti.PausableStorage").PatchAll();
    }
  }
}