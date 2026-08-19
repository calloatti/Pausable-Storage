Include ..\AGENTS.md

# Pausable Storage — Mod-Specific Agent Instructions

## Identity
- **Assembly:** `pausablestorage`
- **Namespace:** `Calloatti.PausableStorage`
- **Framework:** Harmony, Bindito DI
- **ModId:** `Calloatti.PausableStorage`
- **Min Game Version:** 1.0.0.0 — uses `timberborn-decompiled-1.0.*`

## What This Mod Does
Adds pause functionality to storage buildings. Allows players to pause stockpile/warehouse operations (stop accepting goods, stop distribution) via a pause button.

## Source Architecture (`Version-1.0/Source/`)

| File | Role |
|---|---|
| `ModStarter.cs` | Entry point — `IModStarter` |
| `ModConfigurator.cs` | DI configurator |
| `ModComponent.cs` | Core pause component |
| `ModPatches.cs` | Harmony patches for storage pausing |
| `ModPatchesNeeds.cs` | Harmony patches for needs-related pausing |

## Hard Rule
DO NOT EVER TOUCH THE DEPLOY FOLDER.

BUILD DOES EVERYTHING, NEVER EVER MESS WITH THE DEPLOY PROCESS.
