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
    public TMP_Text characterSheetNew;
    public TMP_InputField hPInput;
    public TMP_InputField aPInput;
    public TMP_InputField notesInput;

    public Dictionary<string, string> GetCharacterSheetTexts()
    {
        string masterStatsText = "Stat     \tDesc.      \tCheck\n";
        string newMasterStatsText = "Stat     \tGrade      \tDesc.\n";
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
            newMasterStatsText += s.ToString() + "\t\t" + hero.finalStats[s] + "\t\t" + hero.StatDesc(hero.finalStats[s]) + "\n";
            //statDamageBonusText += " " + s.ToString() + ": " + hero.statDamageBonuses[s];
            //statGainCheckText += " " + s.ToString() + ": " + hero.statGainChecks[s] + "%";
        }
        return new Dictionary<string, string>()
        {
            {
                "Print",
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
                hero.background.description
            },
            {
                "Column A",
                "Hero\n" +
                "Name: " + hero.name + "\n" +
                "Background: " + hero.background.name + "\n" +
                "Level: " + hero.level + "\n" +
                "\n" +
                masterStatsText
            },
            {
                "Column B",
                "\n" +
                "HP:    " + Mathf.Round(hero.currentHP) + "    / " + Mathf.Round(hero.maxHP) + "\n" +
                "Combat Speed: " + Mathf.Round(hero.combatSpeed) + "\n" +
                "Movement Initiative Bonus: " + hero.movementInitiativeBonus + "\n" +
                "Action Points:    " + hero.currentActionPoints + "    / " + hero.maxActionPoints + "\n" +
                "\n" +
                "SKILLS:\n" +
                "Dodge: " + hero.dodge + "%\n" +
                "Melee: " + Mathf.Round(hero.melee) + "%"
            },
            {
                "HP Input",
                Mathf.Round(hero.currentHP).ToString()
            },
            {
                "AP Input",
                Mathf.Round(hero.currentActionPoints).ToString()
            },
            {
                "Notes Input",
                hero.notes
            },
            {
                "NewSheet",
                "CHARACTER SHEET\n" +
                "\n" +
                "Name: " + hero.name + "\n" +
                "Gang: " + hero.team + "\n" +
                "Background: " + hero.background.name + "\n" +
                "Level: " + hero.level + "\n" +
                "\n" +
                newMasterStatsText +
                "\n" +
                "HP: " + Mathf.Round(hero.currentHP) + "/" + Mathf.Round(hero.maxHP) + "\n" +
                "\n" +
                "APs: " + hero.currentActionPoints + "/" + hero.maxActionPoints + "\n" +
                "\n" +
                "CS: " + Mathf.Round(hero.combatSpeed) + "\n" +
                "\n" +
                "MIB: " + hero.movementInitiativeBonus
            }
        };
        
    }

    public void ViewCharacterSheet()
    {
        Dictionary<string, string> sheetTexts = GetCharacterSheetTexts();

        characterSheetNew.text = sheetTexts["NewSheet"];

        characterSheetColumnA.text = sheetTexts["Column A"];
        characterSheetColumnB.text = sheetTexts["Column B"];
        hPInput.text = sheetTexts["HP Input"];
        aPInput.text = sheetTexts["AP Input"];
        notesInput.text = sheetTexts["Notes Input"];
    }

    public void EmptyCharacterSheet()
    {
        characterSheetColumnA.text = "";
        characterSheetColumnB.text = "";
        characterSheetNew.text = "";
    }
}
