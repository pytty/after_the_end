using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPBehaviour : MonoBehaviour
{
    [SerializeField] public FP fp;

    public void ChangeType(Hero.Stat newStat)
    {
        fp.stat = newStat;
        //TO DO: change UI
    }
}
