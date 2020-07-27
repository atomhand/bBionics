using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;

namespace bBionics
{
    [DefOf]
    public static class BStatDefOf
    {
        public static StatDef EnergyShieldEnergyMaxMultiplier;
        public static StatDef EnergyShieldRechargeRateMultiplier;

        static BStatDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(StatDefOf));
        }
    }
}
