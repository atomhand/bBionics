using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using System.Reflection.Emit;
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

			MethodInfo from = AccessTools.Property(typeof(ShieldBelt), "EnergyMax").GetGetMethod(true);
			MethodInfo to = AccessTools.DeclaredMethod(typeof(ShieldBeltUtility), "EnergyMax");
			instructionList = instructionList.MethodReplacer(from, to).ToList();

			from = AccessTools.Property(typeof(ShieldBelt), "EnergyGainPerTick").GetGetMethod(true);
			to = AccessTools.DeclaredMethod(typeof(ShieldBeltUtility), "EnergyGainPerTick");
			instructionList = instructionList.MethodReplacer(from, to).ToList();

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

			MethodInfo from = AccessTools.DeclaredMethod(typeof(StatExtension), "GetStatValue");
			MethodInfo to = AccessTools.DeclaredMethod(typeof(ShieldBeltUtility), "EnergyMax");

			// Find and replace the appropriate method call
			// Trace back from there to remove 2 parameters that we are not using
			for ( int i =0; i<instructionList.Count; i++)
            {
				var method = instructionList[i].operand as MethodBase;
				if (method == from)
				{
					instructionList[i].opcode = to.IsConstructor ? OpCodes.Newobj : OpCodes.Call;
					instructionList[i].operand = to;

					instructionList.RemoveRange(i - 2, 2);
					i -= 2;
				}
			}

			return instructionList;
		}
	}
}