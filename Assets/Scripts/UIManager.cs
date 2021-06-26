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
    public List<Transform> ResInResSelection = new List<Transform>();
    public RectTransform resSelectionBox;
    public TMP_Text resSelectionInfoText;
    public TMP_Text resSelectionCombatSpeedBonus;
    public TMP_Text resSelectionInitiativeTotal;
    private int resIndex;
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
                selectedHero.RemoveFromFPPool((Hero.Stat)System.Enum.Parse(typeof(Hero.Stat), stat));
                RefreshFPPool();
            }
        }
    }

    public void ShowResourceSelectionUI(bool yes)
    {
        if (yes)
            RefreshResourceSelection();
        resourceSelectionUI.SetActive(yes);
        resSelectionBox.gameObject.SetActive(false);
    }

    private void RefreshResourceSelection()
    {
        int initiative = 0;

        //TO DO: copy paste ylempää
        //TO DO: hyi hyi hyi!!!
        //TO DO: vaikka tää on ihan ok niin ainaki voi refactoroida, DRY
        foreach (Transform RT in ResInResSelection)
        {
            foreach (Transform child in RT)
            {
                if (child.name == "FP" || child.name == "RES" || child.name == "EmptyRes Button")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        if (selectedHero != null)
        {
            resSelectionInfoText.text = "Choose " + selectedHero.name + "'s Resources for the next Frame:";

            for (int i = 0; i < selectedHero.maxResSize; i++)
            {
                if (selectedHero.resources.Count > i)
                {
                    if (selectedHero.resources[i] is SpecialResource)
                    {
                        Transform trans = ResInResSelection[i].Find("RES");    
                        trans.GetComponent<ResourceBehaviour>().ChangeType((selectedHero.resources[i] as SpecialResource).type);
                        trans.gameObject.SetActive(true);

                        int bonus;
                        if (((selectedHero.resources[i] as SpecialResource).type) == SpecialResource.SpecialResourceType.MED)
                            bonus = 0;
                        else if (((selectedHero.resources[i] as SpecialResource).type) == SpecialResource.SpecialResourceType.MOVE)
                            bonus = selectedHero.movementInitiativeBonus;
                        else
                            throw new System.Exception("I AM ERROR.");
                        ResInResSelection[i].Find("Text (TMP)").GetComponent<TMP_Text>().text = "+" + bonus;
                        initiative += bonus;
                    }
                    else if (selectedHero.resources[i] is FP)
                    {
                        Hero.Stat stat = (selectedHero.resources[i] as FP).stat;

                        Transform trans = ResInResSelection[i].Find("FP");
                        trans.GetComponent<FPBehaviour>().ChangeType(stat);
                        trans.gameObject.SetActive(true);

                        int bonus = GameRules.initiativeStatBonuses[stat];
                        initiative += bonus;

                        string symbol = (bonus >= 0 ? "+" : "-");
                        ResInResSelection[i].Find("Text (TMP)").GetComponent<TMP_Text>().text = symbol + bonus;

                    }
                    else
                    {
                        throw new System.Exception("I AM ERROR.");
                    }
                }
                else
                {
                    ResInResSelection[i].Find("EmptyRes Button").gameObject.SetActive(true);
                    ResInResSelection[i].Find("Text (TMP)").GetComponent<TMP_Text>().text = "+0";
                    //initiative += 0;
                }
            }
            initiative += (int)Mathf.Round(selectedHero.combatSpeed);
            resSelectionCombatSpeedBonus.text = "+" + (int)Mathf.Round(selectedHero.combatSpeed) + " Combat Speed";
        }
        string rank = "";
        foreach(KeyValuePair<int, string> entry in GameRules.initiativeRankLowerThresholds)
        {
            if (initiative >= entry.Key)
                rank = entry.Value;
            else
                break;
        }
        resSelectionInitiativeTotal.text = "Total: " + initiative + "\n" +
            "Rank: " + rank;
    }

    public void ClickResourceSelected(int index)
    {
        if (battleManager.battle.state == Battle.State.SetRes)
        {
            //TO DO: kovakoodausta
            resSelectionBox.anchoredPosition =
                new Vector3(resSelectionBox.anchoredPosition.x, -80.0f - (index * 60.0f));
            resSelectionBox.gameObject.SetActive(true);
            resIndex = index;
        }
    }

    public void ClickResourceBoxRes(string res)
    {
        if (battleManager.battle.state == Battle.State.SetRes)
        {
            if (selectedHero != null)
            {
                //if new res is FP
                //TO DO: kovakoodausta
                if (res == "STR" || res == "AGI" || res == "WILL")
                {
                    Hero.Stat newStat = (Hero.Stat)System.Enum.Parse(typeof(Hero.Stat), res);
                    //if said FP is available in FPPool
                    if (selectedHero.FPPool.Exists(item => item.stat == newStat))
                    {
                        FP newFP = new FP(newStat);
                        //remove from FPPool
                        selectedHero.RemoveFromFPPool(newStat);
                        RefreshFPPool();
                        //if res spot was occupied, change the old resource
                        if (selectedHero.resources.Count > resIndex)
                        {
                            //if released RES was FP
                            if (selectedHero.resources[resIndex] is FP)
                            {
                                //if there's space in FP Pool
                                if (selectedHero.FPPool.Count < selectedHero.maxFPPoolSize)
                                    //return it to FP Pool
                                    selectedHero.FPPool.Add(selectedHero.resources[resIndex] as FP);
                                else
                                    //otherwise give warning that FP was lost
                                    //this shouldn't happen though?
                                    Debug.Log("FP was lost");
                                RefreshFPPool();
                            }

                            //change the resource
                            selectedHero.resources[resIndex] = newFP;
                        }
                        else
                        { 
                            //or add new resource
                            selectedHero.resources.Add(newFP);
                        }
                    }
                    else
                    {
                        //don't change anything, give warning instead
                        //TO DO: give warning
                        Debug.Log("That FP is not available in your FP Pool.");
                    }

                }
                //if new res is special resource
                else if (res == "MOVE" || res == "MED")
                {
                    SpecialResource newRes = new SpecialResource((SpecialResource.SpecialResourceType)System.Enum.Parse(typeof(SpecialResource.SpecialResourceType), res));
                    //if res spot was occupied, change the old resource
                    if (selectedHero.resources.Count > resIndex)
                    {
                        //if released RES was FP
                        if (selectedHero.resources[resIndex] is FP)
                        {
                            //if there's space in FP Pool
                            if (selectedHero.FPPool.Count < selectedHero.maxFPPoolSize)
                                //return it to FP Pool
                                selectedHero.FPPool.Add(selectedHero.resources[resIndex] as FP);
                            else
                                //otherwise give warning that FP was lost
                                //this shouldn't happen though?
                                Debug.Log("FP was lost");
                            RefreshFPPool();
                        }

                        //change the resource
                        selectedHero.resources[resIndex] = newRes;
                    }
                    else
                    {
                        //or add new resource
                        selectedHero.resources.Add(newRes);
                    }
                }
                else if (res == "")
                {
                    //if res spot was occupied, change the old resource
                    if (selectedHero.resources.Count > resIndex)
                    {
                        //if released RES was FP
                        if (selectedHero.resources[resIndex] is FP)
                        {
                            //if there's space in FP Pool
                            if (selectedHero.FPPool.Count < selectedHero.maxFPPoolSize)
                                //return it to FP Pool
                                selectedHero.FPPool.Add(selectedHero.resources[resIndex] as FP);
                            else
                                //otherwise give warning that FP was lost
                                //this shouldn't happen though?
                                Debug.Log("FP was lost");
                            RefreshFPPool();
                        }

                        selectedHero.resources.RemoveAt(resIndex);
                    }
                }
                else
                {
                    throw new System.Exception("I AM ERROR.");
                }
                RefreshResourceSelection();
                resSelectionBox.gameObject.SetActive(false);
            }
        }
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
