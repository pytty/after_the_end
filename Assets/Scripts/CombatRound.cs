using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRound
{
    private const int numOfFrames = 3;
    public List<CombatFrame> combatFrames;

    public CombatRound()
    {
        combatFrames = new List<CombatFrame>();
        for (int i = 0; i < numOfFrames; i++)
        {
            combatFrames.Add(new CombatFrame());
        }
    }
}
