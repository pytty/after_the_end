﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public int maxNumOfRounds;
    public int defNumOfFocusPoints = 8;
    //TO DO: does C# lists keep order?
    public List<BattleRound> combatRounds = new List<BattleRound>();
}
