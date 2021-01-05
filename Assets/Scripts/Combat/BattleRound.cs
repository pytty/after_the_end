using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRound
{
    private Battle battle;
    public List<BattleFrame> frames;

    public BattleRound()
    {
        frames = new List<BattleFrame>();
        for (int i = 0; i < battle.numOfFrames; i++)
        {
            frames.Add(new BattleFrame());
        }
    }
}
