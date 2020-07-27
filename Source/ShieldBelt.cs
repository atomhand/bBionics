using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace bBionics
{
    static class ShieldBeltUtility
    {
        public static float EnergyMax( ShieldBelt me, StatDef stat = null, bool b = false)
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
    }
}
