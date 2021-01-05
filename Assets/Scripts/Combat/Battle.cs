using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public int maxNumOfRounds;
    public int numOfTicks = 3; //TO DO: const
    public int numOfFrames = 3; //TO DO: const

    public int defNumOfFocusPoints = 8;
    //TO DO: does C# lists keep order?
    public List<BattleRound> rounds = new List<BattleRound>();
}
