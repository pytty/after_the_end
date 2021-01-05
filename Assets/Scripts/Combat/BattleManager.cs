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
        //ROUND
        //
        yield return null;
    }
}
