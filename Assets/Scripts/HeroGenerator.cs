using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroGenerator : MonoBehaviour
{
    public UIManager ui;

    public Hero player;
    public List<Hero.Genes> genes = new List<Hero.Genes>();
    public List<Background> backgrounds = new List<Background>();

    // Start is called before the first frame update
    void Awake()
    {
        //TO DO: fiksumpi implementointi tälle kovakoodaukselle
        genes.Add(new Hero.Genes("Poor", 8, 4));
        genes.Add(new Hero.Genes("Fair", 6, 6));
        genes.Add(new Hero.Genes("Elite", 4, 8));


        List<string> genesOptions = new List<string>();
        foreach (Hero.Genes g in genes)
        {
            genesOptions.Add(g.name);
        }
        ui.genesSelect.AddOptions(genesOptions);

        backgrounds.Add(new Background(
            "Bandit",
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.STR, Hero.Stat.AGI, Hero.Stat.STR },
            new List<Hero.Stat> { Hero.Stat.STR }
            ));
        backgrounds.Add(new Background(
            "Nomad",
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.STR, Hero.Stat.AGI },
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.AGI }
            ));
        backgrounds.Add(new Background(
            "Village Leader",
            new List<Hero.Stat> { Hero.Stat.AGI, Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.WILL },
            new List<Hero.Stat> { Hero.Stat.AGI, Hero.Stat.WILL }
            ));
        backgrounds.Add(new Background(
            "Mercenary",
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.STR, Hero.Stat.WILL },
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.WILL }
            ));
        backgrounds.Add(new Background(
            "Monk",
            new List<Hero.Stat> { Hero.Stat.AGI, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.STR, Hero.Stat.AGI },
            new List<Hero.Stat> { Hero.Stat.AGI }
            ));
        backgrounds.Add(new Background(
            "Shaman",
            new List<Hero.Stat> { Hero.Stat.WILL, Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.WILL },
            new List<Hero.Stat> { Hero.Stat.WILL }
            ));

        List<string> backgroundOptions = new List<string>();
        foreach (Background b in backgrounds)
        {
            string text = b.name + " ( ";
            foreach (Hero.Stat s in b.specialities)
            {
                text += s.ToString() + " ";
            }
            text += ")";
            backgroundOptions.Add(text);
        }
        ui.backgroundSelect.AddOptions(backgroundOptions);
    }

    public Hero CreateNewHero(string name, Background background, int level, Hero.Genes genes)
    {
        Hero hero = new Hero(name, background, level);
        hero.GenerateHero(genes);
        return hero;
    }
}
