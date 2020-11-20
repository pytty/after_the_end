using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background
{
    public string name;
    public List<Hero.Stat> statBonuses;
    public List<Hero.Stat> specialities;
    public string description;

    public Background(string name, List<Hero.Stat> statBonuses, List<Hero.Stat> specialities, string description)
    {
        this.name = name;
        this.statBonuses = statBonuses;
        this.specialities = specialities;
        this.description = description;
    }
}
