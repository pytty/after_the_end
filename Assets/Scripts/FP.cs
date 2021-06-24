using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class FP
{
    public Hero.Stat stat;
    Color color; //RED STR, BLU AGI, WHI WIL

    public FP(Hero.Stat stat)
    {
        this.stat = stat;
    }
}
