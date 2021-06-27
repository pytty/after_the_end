using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Battle
{
    public enum State { Prep, Wait, Ready, SetPool, SetRes, GiveOrd, ExecOrd, Over }
    public State state = State.Prep;

    public List<Hero> heroes = new List<Hero>();

    public int maxNumOfRounds;
    public int numOfTicks = 4; //TO DO: const
    public int numOfFrames = 3; //TO DO: const

    public int defNumOfFocusPoints = 8;
    //TO DO: does C# lists keep order?
    public List<BattleRound> rounds = new List<BattleRound>();
    public int roundIndex = 0;
}
