using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Verse;
using System.Runtime.CompilerServices;
using UnityEngine;

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

		// Patch ShieldBelt to use our EnergyMax & recharge getters
		// Unfortunately the actual getters seem to be getting inlined by the compiler so this is necessary (but could be a transpiler?)

		/*
		static bool Prefix( ref ShieldBelt __instance, ref float ___energy, ref int ___ticksToReset)
        {			
			Patch_ThingWithComps_Tick.MyTick(__instance);

			if (__instance.Wearer == null)
			{
				___energy = 0f;
				return false;
			}
			if (__instance.ShieldState == ShieldState.Resetting)
			{
				___ticksToReset--;
				if (___ticksToReset <= 0)
				{
					// this.Reset();

					// should pre-do this in Prepare
					typeof(ShieldBelt).GetMethod("Reset").Invoke(__instance, null);

					return false;
				}
			}
			else if (__instance.ShieldState == ShieldState.Active)
			{
				___energy += ShieldBeltUtility.EnergyGainPerTick(__instance);
				if (___energy > ShieldBeltUtility.EnergyMax(__instance))
				{
					___energy = ShieldBeltUtility.EnergyMax(__instance);
				}
			}

			return false;
		}
		*/
	}

	/*
    [HarmonyPatch(typeof(Gizmo_EnergyShieldStatus), "GizmoOnGUI")]
    static class Patch_Gizmo_EnergyShieldStatus_GizmoOnGUI
    {
		static bool Prefix( ref Gizmo_EnergyShieldStatus __instance, ref GizmoResult __result, ref  Texture2D ___FullShieldBarTex, ref Texture2D ___EmptyShieldBarTex, Vector2 topLeft, float maxWidth)
		{
			float shieldMax = ShieldBeltUtility.EnergyMax(__instance.shield);

			Rect rect = new Rect(topLeft.x, topLeft.y, __instance.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			rect3.height = rect.height / 2f;
			Text.Font = GameFont.Tiny;
			Widgets.Label(rect3, __instance.shield.LabelCap);
			Rect rect4 = rect2;
			rect4.yMin = rect2.y + rect2.height / 2f;
			float fillPercent = __instance.shield.Energy / Mathf.Max(1f, shieldMax);
			Widgets.FillableBar(rect4, fillPercent, ___FullShieldBarTex, ___EmptyShieldBarTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, (__instance.shield.Energy * 100f).ToString("F0") + " / " + (shieldMax * 100f).ToString("F0"));
			Text.Anchor = TextAnchor.UpperLeft;
			__result = new GizmoResult(GizmoState.Clear);


			return false;
		}
	}
	*/
}