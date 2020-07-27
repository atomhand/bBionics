using RimWorld;
using Verse;

namespace bBionics
{
    static class ShieldBeltUtility
    {
        public static float EnergyMax( ShieldBelt me)
        {
            Pawn wearer = me.Wearer;
            float result = me.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true);
            if( wearer != null )
                result *= wearer.GetStatValueForPawn(BStatDefOf.EnergyShieldEnergyMaxMultiplier, wearer);

            return result;
        }

        public static float EnergyGainPerTick( ShieldBelt me)
        {
            Pawn wearer = me.Wearer;
            float result = me.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true) / 60f;
            if (wearer != null)
                result *= wearer.GetStatValueForPawn(BStatDefOf.EnergyShieldRechargeRateMultiplier, wearer);

            return result;
        }

        public static float FEnergyMax(ShieldBelt me, StatDef stat = null, bool b = false)
        {
            return EnergyMax(me);
        }
    }
}
