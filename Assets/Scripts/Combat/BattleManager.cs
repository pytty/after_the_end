using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] public Battle battle;
    private UIManager ui;
    private ObjectSelector selector;

    private void Awake()
    {
        ui = GetComponent<UIManager>();
        selector = GetComponent<ObjectSelector>();
        battle = new Battle();
        //StartBattle();
    }

    private void Update()
    {

        if (battle.state == Battle.State.SetPool)
        {
            //while character chosen
            //show his fp pool
        }
        else if (battle.state == Battle.State.SetRes)
        {
            //while character chosen
            //show his fp pool and res
        }
    }

    public void StartBattle()
    {
        battle.roundIndex = 0;
        battle.state = Battle.State.SetPool;
        StartCoroutine(BattleSystem());
    }

    private IEnumerator BattleSystem()
    {
        //BEFORE BATTLE
        //set pool
        battle.state = Battle.State.SetPool;
        ui.ShowActionButtonsUI(true);
        ui.ShowActionReadyButton(true);
        if (ui.selectedHero != null && selector.selectedGameObject != null && selector.selectedGameObject.GetComponent<Piece>().hero != null)
        {
            ui.ShowFPPool(true);
            ui.ShowFPPoolSelectUI(true);
        }
        do
        {
            //wait for player to set fp pool
            yield return null;
        } while (battle.state == Battle.State.SetPool);
        ui.ShowActionReadyButton(false);
        ui.ShowFPPoolSelectUI(false);
        BattleRound thisRound;
        BattleFrame thisFrame;
        BattleTick thisTick;
        BattleTurn thisTurn;
        do
        {
            //BEGIN ROUND
            battle.state = Battle.State.Wait;
            battle.rounds.Add(new BattleRound());
            thisRound = battle.rounds[battle.roundIndex];
            do
            {
                //BEGIN FRAME
                battle.state = Battle.State.Wait;
                thisRound.frames.Add(new BattleFrame());
                thisFrame = thisRound.frames[thisRound.frameIndex];
                //set resources for each character, 0-4/remaining APs
                battle.state = Battle.State.SetRes;
                do
                {
                    //wait for player to set resources
                    yield return null;
                } while (battle.state == Battle.State.SetRes);
                //different resources are:
                //red (STR FP), blue (AGI FP), white (WILL FP), green (MP), yellow (meditate)
                do
                {
                    //BEGIN TICK
                    battle.state = Battle.State.Wait;
                    thisFrame.ticks.Add(new BattleTick());
                    thisTick = thisFrame.ticks[thisFrame.tickIndex];
                    //compute initiative order
                    //characters get turns in initiative order
                    do
                    {
                        //BEGIN TURN
                        battle.state = Battle.State.Wait;
                        thisTick.turns.Add(new BattleTurn());
                        thisTurn = thisTick.turns[thisTick.turnIndex];
                        //character can execute 1 action on his/her turn by selecting a resource
                        battle.state = Battle.State.GiveOrd;
                        do
                        {
                            //wait for player to give order
                            yield return null;
                        } while (battle.state == Battle.State.GiveOrd);
                        //possible actions: move, load FP, meditate, wait, use item
                        battle.state = Battle.State.ExecOrd;
                        //execute order
                        battle.state = Battle.State.Wait;
                        //END TURN, next character
                        battle.state = Battle.State.Wait;
                        thisTick.turnIndex++;
                        yield return null;
                    } while (thisTick.turnIndex < thisTick.initiativeOrder.Count);
                    //after every character has got one turn, next tick
                    //END TICK, TICK++ UNTIL 4
                    battle.state = Battle.State.Wait;
                    thisFrame.tickIndex++;
                    yield return null;
                } while (thisFrame.tickIndex < battle.numOfTicks);
                //END FRAME, FRAME++ UNTIL 3
                battle.state = Battle.State.Wait;
                thisRound.frameIndex++;
                yield return null;
            } while (thisRound.frameIndex < battle.numOfFrames);
            //END ROUND, ROUND++ UNTIL out of rounds
            battle.state = Battle.State.Wait;
            battle.roundIndex++;
            yield return null;
        } while (battle.roundIndex < battle.maxNumOfRounds);
        battle.state = Battle.State.Over;
        yield return null;
    }
}
