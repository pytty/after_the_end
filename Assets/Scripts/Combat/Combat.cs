using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat
{
    public int maxNumOfRounds;
    public int defNumOfFocusPoints = 8;
    //TO DO: does C# lists keep order?
    public List<CombatRound> combatRounds = new List<CombatRound>();
}
