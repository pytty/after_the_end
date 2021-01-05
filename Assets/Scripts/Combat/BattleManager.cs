using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Battle battle;

    // Start is called before the first frame update
    void Start()
    {
        battle = new Battle();
    }

    public void StartBattle()
    {
        StartCoroutine(BattleSystem());
    }

    private IEnumerator BattleSystem()
    {
        //BEGIN ROUND
        //BEGIN FRAME
        //set resources for each character, 0-4/remaining APs
        //different resources are red (STR FP), blue (AGI FP), white (WILL FP), green (MP), yellow (meditate)
        //BEGIN TICK
        //compute initiative order
        //characters get turns in initiative order
        //BEGIN TURN
        //character can execute 1 action on his/her turn by selecting a resource
        //possible actions: move, load FP, meditate, wait, use item
        //END TURN
        //after every character has got one turn, next tick
        //END TICK, TICK++ UNTIL 4
        //END FRAME, FRAME++ UNTIL 3
        //END ROUND, ROUND++ UNTIL out of rounds
        yield return null;
    }
}
