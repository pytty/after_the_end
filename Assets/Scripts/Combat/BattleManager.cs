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
        //TO DO: kovakoodausta
        battle.maxNumOfRounds = 8;
        //TO DO: nää selkeempään paikkaan
        battle.roundIndex = 0;
        battle.state = Battle.State.Prep;
        StartCoroutine(BattleStateMachine());
        //StartBattle();
    }

    private void Update()
    {
        //TO DO: tää on hyvä tapa, rajapinnat pysyvät selkeinä
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
        battle.state = Battle.State.SetPool;
        StartCoroutine(BattleSystem());
    }

    private IEnumerator BattleSystem()
    {
        //BEFORE BATTLE
        //set pool
        battle.state = Battle.State.SetPool;
        if (ui.selectedHero != null && selector.selectedGameObject != null && selector.selectedGameObject.GetComponent<Piece>().hero != null)
        {
            ui.ShowFPPool(true);
            ui.ShowFPPoolSelectUI(true);
        }
        ui.ShowActionButtonsUI(true);
        ui.ShowActionReadyButton(true);
        do
        {
            //wait for player to set fp pool
            yield return null;
        } while (battle.state == Battle.State.SetPool);
        ui.ShowFPPoolSelectUI(false);
        ui.ShowActionReadyButton(false);

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
                if (ui.selectedHero != null && selector.selectedGameObject != null && selector.selectedGameObject.GetComponent<Piece>().hero != null)
                {
                    ui.ShowResourceSelectionUI(true);
                }
                ui.ShowActionButtonsUI(true);
                ui.ShowActionReadyButton(true);
                do
                {
                    //wait for player to set resources
                    //different resources are:
                    //red (STR FP), blue (AGI FP), white (WILL FP), green (MP), yellow (meditate)
                    yield return null;
                } while (battle.state == Battle.State.SetRes);
                ui.ShowResourceSelectionUI(false);
                ui.ShowActionReadyButton(false);

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

    private IEnumerator BattleStateMachine()
    {
        string RFTString()
        {
            //TO DO: hyi!
            return
                    "Round:\t" + (battle.roundIndex + 1) + "/" + battle.maxNumOfRounds + "\n" +
                    "Frame:\t" + (battle.rounds[battle.roundIndex].frameIndex + 1) + "/" + battle.numOfFrames + "\n" +
                    "Tick:\t\t" + (battle.rounds[battle.roundIndex].frames[battle.rounds[battle.roundIndex].frameIndex].tickIndex + 1) + "/" + battle.numOfTicks;
        }
        bool end = false;
        do
        {
            if (battle.state == Battle.State.Prep)
            {
                ui.roundFrameTickText.text = "Deployment";
                yield return null;
            }
            else if (battle.state == Battle.State.Wait)
            {
                yield return null;
            }
            else if (battle.state == Battle.State.Ready)
            {
                ui.roundFrameTickText.text = RFTString();
                yield return null;
            }
            else if (battle.state == Battle.State.SetPool)
            {
                ui.roundFrameTickText.text = "Set FP Pools";
                yield return null;
            }
            else if (battle.state == Battle.State.SetRes)
            {
                ui.roundFrameTickText.text = RFTString();
                yield return null;
            }
            else if (battle.state == Battle.State.GiveOrd)
            {
                ui.roundFrameTickText.text = RFTString();
                yield return null;
            }
            else if (battle.state == Battle.State.ExecOrd)
            {
                ui.roundFrameTickText.text = RFTString();
                yield return null;
            }
            else if (battle.state == Battle.State.Over)
            {
                ui.roundFrameTickText.text = RFTString();
                end = true;
                yield return null;
            }
        } while (!end);
    }
}
