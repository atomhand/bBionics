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

	// This is to access the base.Tick() call in ShieldBelt.Tick(). it's a pain but I believe the only way.
	[HarmonyPatch]
	public class Patch_ThingWithComps_Tick
	{
		[HarmonyReversePatch]
		[HarmonyPatch(typeof(ThingWithComps), "Tick")]
		public static void MyTick(object instance)
		{
			// its a stub so it has no initial content
		}
	}

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