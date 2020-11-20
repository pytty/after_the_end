using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject selectedGameObject;
    public GameObject selectIndicator;
    private GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DeselectObject();
            go = GetClickedObject();
            if (go != null && go.GetComponent<Piece>() != null)
                SelectObject(go);
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

    void SelectObject(GameObject go)
    {
        if (selectedGameObject != null)
            DeselectObject();
        selectedGameObject = go;
        selectIndicator.transform.parent = go.transform;
        selectIndicator.transform.position = go.transform.position;
        selectIndicator.SetActive(true);
    }

    void DeselectObject()
    {
        if (selectedGameObject)
        {
            selectIndicator.transform.parent = null;
            selectIndicator.SetActive(false);
            selectedGameObject = null;
        }
    }
}
