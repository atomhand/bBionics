using RimWorld;
using Verse;

namespace bBionics
{
    static class ShieldBeltUtility
    {
        public static float EnergyMax( ShieldBelt shield)
        {
            Pawn wearer = shield.Wearer;
            float result = shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true);
            if( wearer != null )
                result *= wearer.GetStatValueForPawn(BStatDefOf.EnergyShieldEnergyMaxMultiplier, wearer);

            return result;
        }

        public static float EnergyGainPerTick( ShieldBelt shield)
        {
            Pawn wearer = shield.Wearer;
            float result = shield.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true) / 60f;
            if (wearer != null)
                result *= wearer.GetStatValueForPawn(BStatDefOf.EnergyShieldRechargeRateMultiplier, wearer);

            return result;
        }

        // (I was too lazy to remove the unneeded parameters in the replaced Gizmo function)
        public static float FEnergyMax(ShieldBelt shield, StatDef stat, bool b)
        {
            return EnergyMax(shield);
        }
    }
}
