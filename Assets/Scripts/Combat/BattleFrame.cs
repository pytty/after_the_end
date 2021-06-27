using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFrame
{
    public BattleRound round;
    public List<BattleTick> ticks = new List<BattleTick>();
    public int tickIndex = 0;
}
