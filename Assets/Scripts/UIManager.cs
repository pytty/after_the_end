using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private HeroGenerator generator;
    private ObjectSelector objectSelector;
    private BattleManager battleManager;
    public Hero selectedHero = null;

    public UICharacterSheet sheet;
    public GameObject fPPoolUI;
    //TO DO: järkevämpi paikka tälle, ja koolle rajoitus ja alustus tehdään koodissa eikä drag&droppaamalla editorissa
    public List<FPBehaviour> fPsInFPPool = new List<FPBehaviour>();
    public GameObject fPPoolSelectUI;
    public GameObject resourceSelectionUI;
    public GameObject actionButtonsUI;
    public GameObject actionReadyButton;

    public TMP_Text roundFrameTickText;

    public TMP_Dropdown genesSelect;
    public TMP_Dropdown backgroundSelect;
    public TMP_Dropdown levelSelect;
    public TMP_Dropdown teamSelect;
    public TMP_Dropdown gridWidthSelect;
    public TMP_Dropdown gridLengthSelect;
    public TMP_Text exportPopUpText;
    public string newHeroName;
    public int newHeroLevel;
    public Hero.Genes newHeroGenes;
    public Background newHeroBackground;
    public int newHeroTeam;

    public int newGridWidth;
    public int newGridLength;

    private void Awake()
    {
        generator = GetComponent<HeroGenerator>();
        objectSelector = GetComponent<ObjectSelector>();
        battleManager = GetComponent<BattleManager>();

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

    public void SetNewHeroTeam(int val)
    {
        newHeroTeam = val + 1;
    }

    public void SetGridWidth(int val)
    {
        newGridWidth = val + 1;
    }

    public void SetGridLength(int val)
    {
        newGridLength = val + 1;
    }

    public void CreateHero()
    {
        if (newHeroName == null || newHeroName == "")
            newHeroName = "Keijo";
        if (newHeroBackground == null)
            newHeroBackground = generator.backgrounds[0];
        if (newHeroGenes == null)
            newHeroGenes = generator.genes[0];
        if (newHeroTeam < 1)
            newHeroTeam = 1;
        else if (newHeroTeam > 2)
            newHeroTeam = 2;
        //väärin
        Hero pylly = new Hero();
        if (newHeroLevel <= 0 || newHeroLevel > pylly.maxLevel)
            newHeroLevel = 1;

        sheet.hero = generator.CreateNewHero(newHeroName, newHeroBackground, newHeroLevel, newHeroGenes, newHeroTeam);
        sheet.ViewCharacterSheet();
    }

    public void CreateGrid()
    {
        GetComponent<LevelCreator>().CreateGrid(newGridWidth, newGridLength);
    }

    public void ShowFPPool(bool yes)
    {
        //TO DO: sort FP Pool, both in the List<> and in the Editor hierarchy
        if (yes)
            RefreshFPPool();
        fPPoolUI.SetActive(yes);
    }

    private void RefreshFPPool()
    {
        foreach (FPBehaviour FPB in fPsInFPPool)
        {
            FPB.gameObject.SetActive(false);
        }
        if (selectedHero != null)
        {
            for (int i = 0; i < selectedHero.FPPool.Count; i++)
            {
                fPsInFPPool[i].ChangeType(selectedHero.FPPool[i].stat);
                fPsInFPPool[i].gameObject.SetActive(true);
            }
        }
    }

    public void ShowFPPoolSelectUI(bool yes)
    {
        fPPoolSelectUI.SetActive(yes);
    }

    public void ClickFPPoolSelect(string stat)
    {
        //TO DO: stringin heittäminen on paska tapa, erityisesti kun se on kovakoodattu onClickin parametriksi
        // se pitäisi tehdä enumilla, ja se onnistuu vain jos Stat muutetaan classiksi
        // https://answers.unity.com/questions/1549639/enum-as-a-function-param-in-a-button-onclick.html
        if (battleManager.battle.state == Battle.State.SetPool)
        {
            if (selectedHero != null && selectedHero.FPPool.Count < selectedHero.maxFPPoolSize)
            {
                selectedHero.FPPool.Add(new FP((Hero.Stat)System.Enum.Parse(typeof(Hero.Stat), stat)));
                RefreshFPPool();
            }
        }
    }

    public void ClickFPPool(string stat)
    {
        //TO DO: sama juttu, stringin heittäminen on paska tapa, erityisesti kun se on kovakoodattu onClickin parametriksi
        // se pitäisi tehdä enumilla, ja se onnistuu vain jos Stat muutetaan classiksi
        // https://answers.unity.com/questions/1549639/enum-as-a-function-param-in-a-button-onclick.html
        if (battleManager.battle.state == Battle.State.SetPool)
        {
            if (selectedHero != null) 
            {
                //TO DO: sortaa lista
                //Find first occurence of the stat in the list
                int index = selectedHero.FPPool.FindIndex(x => x.stat.ToString() == stat);
                if (index != -1)
                {
                    selectedHero.FPPool.RemoveAt(index);
                    RefreshFPPool();
                }
                else
                {
                    throw new System.Exception("I AM ERROR.");
                }
            }
        }
    }

    public void ShowResourceSelectionUI(bool yes)
    {
        resourceSelectionUI.SetActive(yes);
    }


    public void ShowActionButtonsUI(bool yes)
    {
        actionButtonsUI.SetActive(yes);
    }

    public void ShowActionReadyButton(bool yes)
    {
        actionReadyButton.SetActive(yes);
    }

    public void ClickActionReadyButton()
    {
        if (battleManager.battle.state == Battle.State.SetPool)
        {
            battleManager.battle.state = Battle.State.Ready;
        }
        else if (battleManager.battle.state == Battle.State.SetRes)
        {
            battleManager.battle.state = Battle.State.Ready;
        }
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

            string text = sheet.GetCharacterSheetTexts()["Print"];
            File.WriteAllText(Path.Combine(path, filename), text);
            exportPopUpText.text = "File created at: " + Path.Combine(path, filename);
        }
    }
}
