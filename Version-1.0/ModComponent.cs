using Timberborn.BaseComponentSystem;
using Timberborn.Buildings;

namespace Calloatti.PausableStorage
{
  /// <summary>
  /// A lightweight component that satisfies the IFinishedPausable interface requirement.
  /// This triggers the vanilla engine to activate PausableBuilding and inject PausableBuildingTerminal.
  /// </summary>
  public class PausableStorageComponent : BaseComponent, IFinishedPausable
  {
    // No custom logic needed. The interface itself acts as the flag.
  }
}