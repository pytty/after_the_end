using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTick
{
    private BattleFrame frame;
    public List<Hero> initiativeOrder = new List<Hero>();
    public List<BattleTurn> turns = new List<BattleTurn>();
    public int turnIndex = 0;
}
