using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameRules
{
    public static Dictionary<Hero.Stat, short> initiativeStatBonuses =
        new Dictionary<Hero.Stat, short>()
        {
            { Hero.Stat.STR, 1},
            { Hero.Stat.AGI, 5},
            { Hero.Stat.WILL, 3}
        };

    public static Dictionary<int, string> initiativeRankLowerThresholds =
        new Dictionary<int, string>()
        {
            {0, "Super slow" },
            {26, "Very slow" },
            {33, "Slow" },
            {40, "Average" },
            {47, "Fast" },
            {56, "Very fast" },
            {63, "Super-fast" }
        };
}
