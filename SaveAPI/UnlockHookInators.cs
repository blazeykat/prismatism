using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace ItemAPI
{
    class UnlockHookInators
    {
        public static void AddHooks()
        {
            Hook hook = new Hook(typeof(PlayerStats).GetMethod("RecalculateStatsInternal", BindingFlags.Public | BindingFlags.Instance), typeof(UnlockHookInators).GetMethod("MaxHealthStatAdder"));
        }

        public static void MaxHealthStatAdder(Action<PlayerStats, PlayerController> action, PlayerStats origStats, PlayerController owner)
        {
            action(origStats, owner);
            DaveAPIManager.UpdateMaximum(CustomTrackedMaximums.MAXIMUM_HEALTH, owner.stats.GetStatValue(PlayerStats.StatType.Health));
            DaveAPIManager.UpdateMaximum(CustomTrackedMaximums.MAXIMUM_DAMAGE, owner.stats.GetStatValue(PlayerStats.StatType.Damage));
        }
    }
}
