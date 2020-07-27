using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Verse;

namespace bBionics
{
	[StaticConstructorOnStartup]
	class HarmonyPatches
	{
		private static readonly Type patchType = typeof(HarmonyPatches);
		static HarmonyPatches()
		{
			var harmony = new Harmony("com.ionheart.bMods.bBionics");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}

	// Replace all references to ShieldBlet's EnergyMax and EnergyGainPerTick with my getter functions

	// Replace the getter instances in Tick
	[HarmonyPatch(typeof(ShieldBelt), "Tick")]
	static class Patch_ShieldBelt_Tick
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> instructionList = instructions.ToList();

			MethodInfo miold = AccessTools.Property(typeof(ShieldBelt), "EnergyMax").GetGetMethod(true);
			MethodInfo minew = AccessTools.DeclaredMethod(typeof(ShieldBeltUtility), "EnergyMax");
			instructionList = instructionList.MethodReplacer(miold, minew).ToList();

			miold = AccessTools.Property(typeof(ShieldBelt), "EnergyGainPerTick").GetGetMethod(true);
			minew = AccessTools.DeclaredMethod(typeof(ShieldBeltUtility), "EnergyGainPerTick");
			instructionList = instructionList.MethodReplacer(miold, minew).ToList();

			return instructionList;
		}
	}

	// Replace the direct call to statdef in the energy shield gizmo
    [HarmonyPatch(typeof(Gizmo_EnergyShieldStatus), "GizmoOnGUI")]
    static class Patch_Gizmo_EnergyShieldStatus_GizmoOnGUI
    {
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> instructionList = instructions.ToList();

			MethodInfo miold = AccessTools.DeclaredMethod(typeof(StatExtension), "GetStatValue");
			MethodInfo minew = AccessTools.DeclaredMethod(typeof(ShieldBeltUtility), "FEnergyMax");
			instructionList = instructionList.MethodReplacer(miold, minew).ToList();

			return instructionList;
		}
	}
}