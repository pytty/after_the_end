using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICharacterSheet : MonoBehaviour
{
    public Hero hero;
    public TMP_Text characterSheetColumnA;
    public TMP_Text characterSheetColumnB;
    public TMP_InputField hPInput;
    public TMP_InputField aPInput;

    public enum ViewPort { LANDSCAPE, PORTRAIT, HUD}
    public ViewPort viewPort = ViewPort.PORTRAIT;

    public void PrintCharacterSheet()
    {
        string masterStatsText = "Stat     \tDesc.      \tCheck\n";
        string rolledStatsText = "Original Stats:\n";
        string baseStatsText = "Base Stats:\n";
        string finalStatsText = "Final Stats:\n";
        //string statDamageBonusText = "Damage Bonus:\n";
        //string statGainCheckText = "Gain Check:\n";
        foreach (Hero.Stat s in Enum.GetValues(typeof(Hero.Stat)))
        {
            masterStatsText += s.ToString() + " " + hero.finalStats[s] + " \t" + hero.StatDesc(hero.finalStats[s]) + " \t" + hero.statGainChecks[s] + "%\n";
            rolledStatsText += " " + s.ToString() + ": " + hero.rolledStats[s];
            baseStatsText += " " + s.ToString() + ": " + hero.baseStats[s];
            finalStatsText += " " + s.ToString() + ": " + hero.finalStats[s];
            //statDamageBonusText += " " + s.ToString() + ": " + hero.statDamageBonuses[s];
            //statGainCheckText += " " + s.ToString() + ": " + hero.statGainChecks[s] + "%";
        }
        if (viewPort == ViewPort.PORTRAIT)
        {
            characterSheetColumnA.text =
                "Character Sheet\n" +
                "\n" +
                "Name: " + hero.name + "\n" +
                "Background: " + hero.background.name + "\n" +
                "Level: " + hero.level + "\n" +
                "\n" +
                masterStatsText + "\n" +
                "\n" +
                rolledStatsText + "\n" +
                baseStatsText + "\n" +
                finalStatsText + "\n" +
                "\n" +
                "HP: " + Mathf.Round(hero.maxHP) + "\n" +
                "\n" +
                "Combat Speed: " + Mathf.Round(hero.combatSpeed) + "\n" +
                "\n" +
                "Movement Initiative Bonus: " + hero.movementInitiativeBonus + "\n" +
                "Action Points: " + hero.maxActionPoints + "\n" +
                "\n" +
                "SKILLS:\n" +
                "Dodge: " + hero.dodge + "%\n" +
                "Melee: " + Mathf.Round(hero.melee) + "%\n" +
                "\n" +
                "Story:\n" +
                hero.background.description;
        }
        else if (viewPort == ViewPort.HUD)
        {
            characterSheetColumnA.text =
                "Hero\n" +
                "Name: " + hero.name + "\n" +
                "Background: " + hero.background.name + "\n" +
                "Level: " + hero.level + "\n" +
                "\n" +
                masterStatsText;

            characterSheetColumnB.text =
                "\n" +
                "HP:    " + Mathf.Round(hero.currentHP) + "    / " + Mathf.Round(hero.maxHP) + "\n" +
                "Combat Speed: " + Mathf.Round(hero.combatSpeed) + "\n" +
                "Movement Initiative Bonus: " + hero.movementInitiativeBonus + "\n" +
                "Action Points:    " + hero.currentActionPoints + "    / " + hero.maxActionPoints + "\n" +
                "\n" +
                "SKILLS:\n" +
                "Dodge: " + hero.dodge + "%\n" +
                "Melee: " + Mathf.Round(hero.melee) + "%";
            hPInput.text = Mathf.Round(hero.currentHP).ToString();
            aPInput.text = Mathf.Round(hero.currentActionPoints).ToString();
        }
    }

    public void EmptyCharacterSheet()
    {
        characterSheetColumnA.text = "";
        characterSheetColumnB.text = "";
    }
}
