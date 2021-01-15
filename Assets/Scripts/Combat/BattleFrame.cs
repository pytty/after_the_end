using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFrame
{
    private BattleRound round;
    public List<BattleTick> ticks = new List<BattleTick>();
    public int tickIndex = 0;
}
