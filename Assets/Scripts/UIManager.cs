﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public HeroGenerator generator;
    public UICharacterSheet sheet;
    public TMP_Dropdown genesSelect;
    public TMP_Dropdown backgroundSelect;
    public TMP_Dropdown levelSelect;
    public TMP_Text exportPopUpText;
    public string newHeroName;
    public int newHeroLevel;
    public Hero.Genes newHeroGenes;
    public Background newHeroBackground;

    private void Awake()
    {
        List<string> levels = new List<string>();
        //väärin
        Hero pylly = new Hero();
        for (int i = 0; i < pylly.maxLevel; i++)
        {
            levels.Add((i + 1).ToString());
        }
        levelSelect.AddOptions(levels);
    }

    public void SetNewHeroName(string val)
    {
        newHeroName = val;
    }

    public void SetNewHeroGenes(int val)
    {
        newHeroGenes = generator.genes[val];
    }

    public void SetNewHeroBackground(int val)
    {
        newHeroBackground = generator.backgrounds[val];
    }

    public void SetNewHeroLevel(int val)
    {
        newHeroLevel = val + 1;
    }

    public void CreateHero()
    {
        if (newHeroName == null || newHeroName == "")
            newHeroName = "Keijo";
        if (newHeroBackground == null)
            newHeroBackground = generator.backgrounds[0];
        if (newHeroGenes == null)
            newHeroGenes = generator.genes[0];
        if (newHeroLevel <= 0 || newHeroLevel >= 100)
            newHeroLevel = 1;

        sheet.hero = generator.CreateNewHero(newHeroName, newHeroBackground, newHeroLevel, newHeroGenes);
        sheet.PrintCharacterSheet();
    }

    public void ExportCharacterSheet()
    {
        string path = "";
        bool failed = false;
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer)
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"after_the_end");
        else if (Application.platform == RuntimePlatform.Android)
            path = Application.persistentDataPath;
        else
            failed = true;
        if (!failed)
        {
            Directory.CreateDirectory(path);
            string fileDefaultName = @"Hero_";
            string id = sheet.hero.name + @"_";
            string extraId;
            string fileExtension = @".txt";
            string filename;

            int i = 0;
            do
            {
                if (i < 10)
                    extraId = @"0" + i.ToString();
                else
                    extraId = i.ToString();
                filename = fileDefaultName + id + extraId + fileExtension;
                i++;
            } while (File.Exists(Path.Combine(path, filename)));

            string text = sheet.characterSheetText.text;
            File.WriteAllText(Path.Combine(path, filename), text);
            exportPopUpText.text = "File created at: " + Path.Combine(path, filename);
        }
    }
}
