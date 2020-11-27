using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject selectedGameObject;
    public GameObject selectIndicator;
    public UICharacterSheet sheet;
    private GameObject go;

    // Start is called before the first frame update
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
            if (go != null && go.GetComponent<Tile>() != null)
                DeselectObject();
            else if (go != null && go.GetComponent<Piece>() != null)
            {
                DeselectObject();
                SelectObject(go);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selectedGameObject)
            {
                go = GetClickedObject();
                if (go != null)
                {
                    Tile tile = go.GetComponent<Tile>();
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

        sheet.gameObject.SetActive(true);
        sheet.hero = selectedGameObject.GetComponent<Piece>().hero;
        sheet.ViewCharacterSheet();
    }

    public void DeselectObject()
    {
        selectIndicator.transform.parent = null;
        selectIndicator.SetActive(false);
        selectedGameObject = null;

        sheet.gameObject.SetActive(false);
        sheet.EmptyCharacterSheet();
    }

    public void EditHeroHP(string hp)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.GetComponent<Piece>().hero.currentHP = float.Parse(hp);
            sheet.ViewCharacterSheet();
        }
    }

    public void EditHeroAP(string ap)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.GetComponent<Piece>().hero.currentActionPoints = int.Parse(ap);
            sheet.ViewCharacterSheet();
        }
    }
    public void EditHeroNotes(string notes)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.GetComponent<Piece>().hero.notes = notes;
            sheet.ViewCharacterSheet();
        }
    }
}
