using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject selectedGameObject;
    public GameObject selectIndicator;
    private UIManager uIManager;
    private GameObject go;
    private BattleManager battleManager;

    private void Awake()
    {
        uIManager = GetComponent<UIManager>();
        battleManager = GetComponent<BattleManager>();
    }

    void Start()
    {
        DeselectObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            go = GetClickedObject();
            /*if (go != null && go.GetComponent<Square>() != null)
                DeselectObject();
            else*/ if (go != null && go.GetComponent<Piece>() != null)
            {
                DeselectObject();
                SelectObject(go);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            DeselectObject();

        //TO DO: tää pitää poistaa
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedGameObject)
            {
                go = GetClickedObject();
                if (go != null)
                {
                    Square tile = go.GetComponent<Square>();
                    if (tile != null)
                    {
                        Piece piece = selectedGameObject.GetComponent<Piece>();
                        if (piece != null)
                        {
                            piece.currentTile = tile;
                            piece.MoveToCurrentTile();
                        }
                        else
                            throw new System.Exception("I AM ERROR.");
                    }
                }
            }
        }
    }

    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            Transform t = hit.transform;
            if (t != null)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public void SelectObject(GameObject go)
    {
        if (selectedGameObject != null)
            DeselectObject();
        selectedGameObject = go;
        selectIndicator.transform.parent = go.transform;
        selectIndicator.transform.position = go.transform.position;
        selectIndicator.SetActive(true);

        uIManager.selectedHero = selectedGameObject.GetComponent<Piece>().hero;

        uIManager.sheet.gameObject.SetActive(true);
        uIManager.sheet.hero = uIManager.selectedHero;
        uIManager.sheet.ViewCharacterSheet();


        if (battleManager.battle.state != Battle.State.Prep)
        {
            uIManager.ShowFPPool(true);
        }

        if (battleManager.battle.state == Battle.State.SetPool)
        {
            uIManager.ShowFPPoolSelectUI(true);
        }

        if (battleManager.battle.state == Battle.State.SetRes)
        {
            uIManager.ShowResSelUI(true);
        }

        if (battleManager.battle.state == Battle.State.ExecOrd ||
            battleManager.battle.state == Battle.State.GiveOrd)
        {
            uIManager.ShowResUI(true);
        }

        //activate "delete hero" button if battle not started
        if (battleManager.battle.state == Battle.State.Prep)
        {
            GameObject kulli = GameObject.Find("Canvas/Side Buttons/Delete Hero Button");
            if (kulli != null)
                kulli.SetActive(true);
        }

    }

    public void DeselectObject()
    {
        selectIndicator.transform.parent = null;
        selectIndicator.SetActive(false);
        selectedGameObject = null;

        uIManager.selectedHero = null;
        uIManager.sheet.gameObject.SetActive(false);
        uIManager.sheet.EmptyCharacterSheet();

        uIManager.ShowFPPool(false);
        uIManager.ShowFPPoolSelectUI(false);
        uIManager.ShowResSelUI(false);
        uIManager.ShowResUI(false);

        //deactivate "delete hero" button if battle not started
        if (battleManager.battle.state == Battle.State.Prep)
        {
            GameObject kulli = GameObject.Find("Canvas/Side Buttons/Delete Hero Button");
            if (kulli != null)
                kulli.SetActive(false);
        }
    }

    public void DeletePiece()
    {
        GameObject temp = selectedGameObject;
        if (temp.GetComponent<Piece>() != null)
        {
            battleManager.battle.heroes.Remove(temp.GetComponent<Piece>().hero);
            DeselectObject();
            Destroy(temp);
        }
    }

    public void EditHeroHP(string hp)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.GetComponent<Piece>().hero.currentHP = float.Parse(hp);
            uIManager.sheet.ViewCharacterSheet();
        }
    }

    public void EditHeroAP(string ap)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.GetComponent<Piece>().hero.currentActionPoints = int.Parse(ap);
            uIManager.sheet.ViewCharacterSheet();
        }
    }
    public void EditHeroNotes(string notes)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.GetComponent<Piece>().hero.notes = notes;
            uIManager.sheet.ViewCharacterSheet();
        }
    }
}
