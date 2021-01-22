using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroGenerator : MonoBehaviour
{
    private UIManager ui;
    public GameObject piecePrefab;
    public List<Piece> heroes = new List<Piece>();

    public List<Hero.Genes> genes = new List<Hero.Genes>();
    public List<Background> backgrounds = new List<Background>();

    // Start is called before the first frame update
    void Awake()
    {
        ui = GetComponent<UIManager>();
        InitializeHeroGenerator();
        if (ui != null)
        {
            List<string> genesOptions = new List<string>();
            foreach (Hero.Genes g in genes)
            {
                genesOptions.Add(g.name + " (d" + g.rollD + "+" + g.constantBonus + ")");
            }
            ui.genesSelect.AddOptions(genesOptions);

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

            if (ui.teamSelect != null)
            {
                List<string> teamOptions = new List<string>();
                for (int i = 0; i < 2; i++)
                {
                    teamOptions.Add((i + 1).ToString());
                }
                ui.teamSelect.AddOptions(teamOptions);
            }
        }
    }

    private void InitializeHeroGenerator()
    {
        //TO DO: fiksumpi implementointi tälle kovakoodaukselle
        genes.Add(new Hero.Genes("Poor", 8, 4));
        genes.Add(new Hero.Genes("Fair", 6, 6));
        genes.Add(new Hero.Genes("Elite", 4, 8));

        backgrounds.Add(new Background(
            "Bandit",
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.STR, Hero.Stat.AGI, Hero.Stat.STR },
            new List<Hero.Stat> { Hero.Stat.STR },
            "You belong to the despicable group of marauders roaming around the land robbing unsuspecting and " +
            "defenseless communities. You live by exploiting others and for that they hate your guts. You revel " +
            "in violence and you have killed many in a berserk state caused by damage in your genes."
            ));
        backgrounds.Add(new Background(
            "Nomad",
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.STR, Hero.Stat.AGI },
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.AGI },
            "You belonged to a clan of wandering nomads who gained their livelihood from herding animals. Your " +
            "strength grew starkly to the point of you becoming paranoid and a threat to your clan, so they " +
            "exiled you into the wilderness. You survived by hunting and gathering until you faced something " +
            "unexpected in the wild."
            ));
        backgrounds.Add(new Background(
            "Village Leader",
            new List<Hero.Stat> { Hero.Stat.AGI, Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.WILL },
            new List<Hero.Stat> { Hero.Stat.AGI, Hero.Stat.WILL },
            "You were the leader of a community of farmers. You were wise and respected, but your greed for " +
            "power made you use the means of fraud and deceit to reach your ambitions. In your tyranny you " +
            "caused hatred, so the farmers exiled you to wander alone in the hostile world. You have barely " +
            "survived by terrorizing farmers living on the fringe of the wilderness."
            ));
        backgrounds.Add(new Background(
            "Mercenary",
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.STR, Hero.Stat.WILL },
            new List<Hero.Stat> { Hero.Stat.STR, Hero.Stat.WILL },
            "From an early age your mutations made you powerful but dangerous. Your pursuit to exploit the " +
            "weakness of others encouraged you to join a band of soldiers of fortune. As part of that crew, " +
            "you sold your services to greedy and malign people. After a job gone wrong your cohort was wiped " +
            "out by a rival troop forcing you, the lone survivor, to escape into the wilderness."
            ));
        backgrounds.Add(new Background(
            "Monk",
            new List<Hero.Stat> { Hero.Stat.AGI, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.STR, Hero.Stat.AGI },
            new List<Hero.Stat> { Hero.Stat.AGI },
            "You have been trained by monks, who practice the art of knowing and sensing the balance between " +
            "the physical and psychic worlds. You were trained in science and self-defense. After spending years " +
            "with the monks, you understood there is more to life than just reading books and meditating. You " +
            "wandered into the wilderness where you suddenly understood something important"
            ));
        backgrounds.Add(new Background(
            "Shaman",
            new List<Hero.Stat> { Hero.Stat.WILL, Hero.Stat.STR, Hero.Stat.WILL, Hero.Stat.AGI, Hero.Stat.WILL },
            new List<Hero.Stat> { Hero.Stat.WILL },
            "You master the art of divination and wandering in the psychic dimension by hidden secrets. Your " +
            "ambition to understand the world has driven you into madness. Dark have your paths been. You " +
            "retreated to the wilderness to understand its psychic nature that so strongly attracts you."
            ));
    }

    public Hero CreateNewHero(string name, Background background, int level, Hero.Genes genes, int team)
    {
        Hero hero = new Hero(name, background, level);
        hero.GenerateHero(genes);
        hero.team = team;
        GameObject go = Instantiate(piecePrefab);
        go.GetComponent<Piece>().hero = hero;

        Color teamColor = Color.blue;
        if (hero.team == 1)
            teamColor = Color.blue;
        else if (hero.team == 2)
            teamColor = Color.red;
        go.GetComponent<Renderer>().material.SetColor("_Color", teamColor);
        GetComponent<ObjectSelector>().SelectObject(go);

        return hero;
    }
}
