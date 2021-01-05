using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRound
{
    private const int numOfFrames = 3;
    public List<BattleFrame> combatFrames;

    public CombatRound()
    {
        combatFrames = new List<BattleFrame>();
        for (int i = 0; i < numOfFrames; i++)
        {
            combatFrames.Add(new BattleFrame());
        }
    }
}
