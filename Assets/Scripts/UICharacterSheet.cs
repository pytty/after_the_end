using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICharacterSheet : MonoBehaviour
{
    public Hero hero;
    public TMP_Text characterSheetText;

    public void PrintCharacterSheet()
    {
        string rolledStatsText = "Original Stats:\n";
        string baseStatsText = "Base Stats:\n";
        string finalStatsText = "Final Stats:\n";
        string statDamageBonusText = "Damage Bonus:\n";
        string statGainCheckText = "Gain Check:\n";
        foreach (Hero.Stat s in Enum.GetValues(typeof(Hero.Stat)))
        {
            rolledStatsText += " " + s.ToString() + ": " + hero.rolledStats[s];
            baseStatsText += " " + s.ToString() + ": " + hero.baseStats[s];
            finalStatsText += " " + s.ToString() + ": " + hero.finalStats[s];
            statDamageBonusText += " " + s.ToString() + ": " + hero.statDamageBonuses[s];
            statGainCheckText += " " + s.ToString() + ": " + hero.statGainChecks[s];
        }


        characterSheetText.text =
            "Character Sheet\n" +
            "\n" +
            "Name: " + hero.name + "\n" +
            "Background: " + hero.background.name + "\n" +
            "Level: " + hero.level + "\n" +
            "\n" +
            rolledStatsText + "\n" +
            baseStatsText + "\n" +
            finalStatsText + "\n" +
            "\n" +
            "HP: " + hero.maxHP + "\n" +
            "Combat Speed: " + hero.combatSpeed + "\n" +
            "Action Points: " + hero.maxActionPoints + "\n" +
            "Movement Initiative Bonus: " + hero.movementInitiativeBonus + "\n" +
            "Dodge: " + hero.dodge + "\n" +
            "Melee: " + hero.melee + "\n" +
            statDamageBonusText + "\n" +
            statGainCheckText;
    }
}
