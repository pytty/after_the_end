using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable] public class Hero
{
    public class Genes
    {
        public string name;
        public int rollD;
        public int constantBonus;

        public Genes(string name, int rollD, int constantBonus)
        {
            this.name = name;
            this.rollD = rollD;
            this.constantBonus = constantBonus;
        }
    };

    public enum Stat { STR, AGI, WILL }

    public string name;
    public int team;
    public Background background;
    public int maxLevel = 50; //make const
    public int level;
    public Dictionary<Stat, int> rolledStats = new Dictionary<Stat, int>(); //TO DO: final/const etc
    public Dictionary<Stat, int> baseStats = new Dictionary<Stat, int>();
    public Dictionary<Stat, int> finalStats = new Dictionary<Stat, int>();

    public float maxHP; //TO DO: pyöristys? onko float vai int?
    public float currentHP;
    public float combatSpeed; //TO DO: pyöristys? onko float vai int?
    public int maxActionPoints;
    public int currentActionPoints;
    public int movementInitiativeBonus;
    public int dodge;

    public Dictionary<Stat, int> statDamageBonuses = new Dictionary<Stat, int>();
    public Dictionary<Stat, int> statGainChecks = new Dictionary<Stat, int>();

    public float melee; //TO DO: pyöristys? onko float vai int?

    public int maxFPPoolSize = 8; //TO DO: järkevämpi paikka tälle
    public List<FP> FPPool = new List<FP>();
    public List<CombatCombo> combatCombos = new List<CombatCombo>();
    public string notes;

    //TO DO: missä luodaan random instance
    System.Random rand = new System.Random();

    public Hero()
    {
    }

    public Hero(string name, Background background, int level)
    {
        this.name = name;
        this.background = background;
        this.level = (level < maxLevel) ? level : maxLevel;
    }

    public void GenerateHero(Genes genes)
    {
        //TO DO: tee tästä update tyyppinen versio ja tsekkaa että alifunktiot toimii myös updatevana (ettei tule esim duplikaatteja)
        //Roll stats
        foreach (Stat s in Enum.GetValues(typeof(Stat)))
        {
            int val = RollStat(genes);
            rolledStats[s] = val;
            baseStats[s] = val;
            finalStats[s] = val;
            statDamageBonuses[s] = 0;
            statGainChecks[s] = 0;
        }

        //level boundaries
        if (level <= 0)
            level = 1;
        else if (level > maxLevel)
            level = maxLevel;

        //Level Bonuses for stats
        if (level > 1)
        {
            int c = 0;
            int len = background.statBonuses.Count;
            for (int i = 1; i < level; i++)
            {
                baseStats[background.statBonuses[c]] += 1;
                c++;
                if (c >= len)
                    c = 0;
            }
        }

        //Stat bonuses from other stats
        foreach (KeyValuePair<Stat, int> bs in baseStats)
        {
            Stat increasingStat = IncreasingStat(bs.Key);
            //Only STR gives bonus
            int bonus = (bs.Key == Stat.STR) ? StatBonusFromOtherStat(bs.Value) : 0;
            finalStats[increasingStat] = baseStats[increasingStat] + bonus;
        }

        //derived stats
        maxHP = CalculateMaxHP();
        currentHP = maxHP;
        combatSpeed = CalculateCombatSpeed();
        maxActionPoints = CalculateMaxActionPoints();
        currentActionPoints = maxActionPoints;
        movementInitiativeBonus = CalculateMovementInitiativeBonus();

        //dodge
        dodge = CalculateDodge();

        //stat damage bonuses
        CalculateStatDamageBonuses();

        //gain checks
        CalculateStatGainChecks();

        //skills
        melee = CalculateMelee();
        Debug.Log("Hero created");
    }

    private int RollStat(Genes genes)
    {
        return rand.Next(1, genes.rollD + 1) + genes.constantBonus;
    }

    private Stat IncreasingStat(Stat other)
    {
        //TO DO: ruma toteutus, eikä tätä kuuluisi kova koodata
        if (other == Stat.STR)
            return Stat.WILL;
        else if (other == Stat.WILL)
            return Stat.AGI;
        else if (other == Stat.AGI)
            return Stat.STR;
        else
            throw new Exception();
    }

    private int StatBonusFromOtherStat(int other)
    {
        //TO DO: ruma toteutus, eikä tätä kuuluisi kova koodata
        int val = 0;
        if (other > 5)
            val = 1;
        if (other > 9)
            val = 2;
        if (other > 13)
            val = 3;
        if (other > 16)
            val = 4;
        if (other > 19)
            val = 5;
        if (other > 22)
            val = 6;
        if (other > 25)
            val = 7;
        if (other > 27)
            val = 8;
        if (other > 29)
            val = 9;
        return val;
    }

    private float CalculateMaxHP()
    {
        float val =
            (
                (float)finalStats[Stat.STR] * 3.5f +
                (float)finalStats[Stat.WILL] * 1.875f +
                (float)finalStats[Stat.AGI] * 0.625f
            ) * 1.5f;
        return val;
    }

    private float CalculateCombatSpeed()
    {
        float val =
            (float)finalStats[Stat.AGI] * 1.25f +
            (float)finalStats[Stat.WILL] * 0.75f;
        return val;
    }

    private int CalculateMaxActionPoints()
    {
        int val = 1;
        if (combatSpeed > 8)
            val = 2;
        if (combatSpeed > 14)
            val = 3;
        if (combatSpeed > 17)
            val = 4;
        if (combatSpeed > 20)
            val = 5;
        if (combatSpeed > 23)
            val = 6;
        if (combatSpeed > 26)
            val = 7;
        if (combatSpeed > 29)
            val = 8;
        if (combatSpeed > 35)
            val = 9;
        if (combatSpeed > 41)
            val = 10;
        if (combatSpeed > 44)
            val = 11;
        if (combatSpeed > 47)
            val = 12;
        return val;
    }

    private int CalculateMovementInitiativeBonus()
    {
        int val = 1;
        if (combatSpeed > 11)
            val = 2;
        if (combatSpeed > 20)
            val = 3;
        if (combatSpeed > 26)
            val = 4;
        if (combatSpeed > 32)
            val = 5;
        if (combatSpeed > 38)
            val = 6;
        if (combatSpeed > 44)
            val = 7;
        if (combatSpeed > 50)
            val = 8;
        return val;
    }

    private int CalculateDodge()
    {
        int val = 0;
        if (finalStats[Stat.AGI] > 1)
            val = 5;
        if (finalStats[Stat.AGI] > 4)
            val = 10;
        if (finalStats[Stat.AGI] > 8)
            val = 15;
        if (finalStats[Stat.AGI] > 10)
            val = 20;
        if (finalStats[Stat.AGI] > 12)
            val = 25;
        if (finalStats[Stat.AGI] > 14)
            val = 30;
        if (finalStats[Stat.AGI] > 15)
            val = 35;
        if (finalStats[Stat.AGI] > 17)
            val = 40;
        if (finalStats[Stat.AGI] > 19)
            val = 45;
        if (finalStats[Stat.AGI] > 21)
            val = 50;
        return val;
    }

    private void CalculateStatDamageBonuses()
    {
        int SDB(int stat)
        {
            int val = 2;
            if (stat > 1)
                val = 3;
            if (stat > 2)
                val = 4;
            if (stat > 3)
                val = 5;
            if (stat > 4)
                val = 6;
            if (stat > 5)
                val = 7;
            if (stat > 6)
                val = 8;
            if (stat > 7)
                val = 9;
            if (stat > 8)
                val = 10;
            if (stat > 9)
                val = 12;
            if (stat > 10)
                val = 14;
            if (stat > 11)
                val = 16;
            if (stat > 12)
                val = 18;
            if (stat > 13)
                val = 20;
            if (stat > 14)
                val = 22;
            if (stat > 15)
                val = 24;
            if (stat > 16)
                val = 26;
            if (stat > 17)
                val = 28;
            if (stat > 18)
                val = 30;
            if (stat > 19)
                val = 32;
            if (stat > 20)
                val = 34;
            if (stat > 21)
                val = 35;
            if (stat > 22)
                val = 36;
            if (stat > 23)
                val = 37;
            if (stat > 24)
                val = 38;
            if (stat > 25)
                val = 40;
            if (stat > 26)
                val = 42;
            if (stat > 27)
                val = 44;
            if (stat > 28)
                val = 47;
            if (stat > 29)
                val = 50;
            return val;
        }

        //TO DO: tyhjennä ensin. vai tarviiko
        foreach (KeyValuePair<Stat, int> s in finalStats)
        {
            statDamageBonuses[s.Key] = SDB(s.Value);
        }
    }

    private void CalculateStatGainChecks()
    {
        int GC(int stat)
        {
            int val = 20;
            if (stat > 3)
                val = 25;
            if (stat > 6)
                val = 30;
            if (stat > 8)
                val = 35;
            if (stat > 10)
                val = 40;
            if (stat > 12)
                val = 45;
            if (stat > 14)
                val = 50;
            if (stat > 16)
                val = 55;
            if (stat > 17)
                val = 60;
            if (stat > 18)
                val = 65;
            if (stat > 19)
                val = 70;
            if (stat > 20)
                val = 75;
            if (stat > 21)
                val = 80;
            if (stat > 22)
                val = 85;
            if (stat > 23)
                val = 90;
            return val;
        }

        //TO DO: tyhjennä ensin. vai tarviiko
        foreach (KeyValuePair<Stat, int> s in finalStats)
        {
            statGainChecks[s.Key] = GC(s.Value);
        }
    }

    private float CalculateMelee()
    {
        float melee =
            (float)finalStats[Stat.STR] * 1.25f +
            (float)finalStats[Stat.AGI] * 1.25f +
            (float)finalStats[Stat.WILL] * 1.5f;
        return melee;
    }

    public string StatDesc(int stat)
    {
        string val = "morbid";
        if (stat > 4)
            val = "poor";
        if (stat > 7)
            val = "average";
        if (stat > 12)
            val = "tough";
        if (stat > 16)
            val = "Badass";
        if (stat > 21)
            val = "Kingpin";
        if (stat > 25)
            val = "Divine";
        return val;
    }
}
