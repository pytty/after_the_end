using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTick
{
    public BattleFrame frame;
    public List<int> initiatives = new List<int>();
    public List<Hero> initiativeOrder = new List<Hero>();
    public List<BattleTurn> turns = new List<BattleTurn>();
    public int turnIndex = 0;


    public void ComputeInitiativeOrder()
    {
        List<int> FindOrderBiggestToSmallestBasedOn(List<Hero> heroes, List<int> attributes)
        {
            if (heroes.Count != attributes.Count)
                throw new System.Exception("I AM ERROR.");


            List<int> candIndices = new List<int>();
            List<int> finalList = new List<int>();
            while (finalList.Count < heroes.Count)
            {
                int bV = -99999;
                candIndices.Clear();
                for (int i = 0; i < heroes.Count; i++)
                {

                    if (!finalList.Contains(i))
                    {
                        if (attributes[i] == bV)
                        {
                            candIndices.Add(i);
                        }
                        else if (attributes[i] > bV)
                        {
                            candIndices.Clear();
                            candIndices.Add(i);
                            bV = attributes[i];
                        }
                    }
                }
                //add to list
                foreach (int x in candIndices) finalList.Add(x);
            }
            foreach (int x in finalList )Debug.Log(x);
            return finalList;
        }

        initiatives.Clear();
        initiativeOrder.Clear();

        //calculate all initiatives
        //TO DO: vähän vittumaisesti joutu muuttaa noi fieldit publiceiksi
        foreach (Hero h in frame.round.battle.heroes)
        {
            initiatives.Add(h.CalculateInitiative());
        }

        List<int> order = FindOrderBiggestToSmallestBasedOn(frame.round.battle.heroes, initiatives);
        //TO DO: ties
        //compare combat speeds
        //compare AGIs
        //compare WILLs
        //compare STRs
        foreach (int x in order)
        {
            initiativeOrder.Add(frame.round.battle.heroes[x]);
        }
    }
}
