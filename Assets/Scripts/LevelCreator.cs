using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Color lightColor;
    public Color darkColor;
    private GameObject tiles;

    private void Awake()
    {
        UIManager ui = GetComponent<UIManager>();
        List<string> gridOptions = new List<string>();
        for (int i = 0; i < 64; i++)
        {
            gridOptions.Add((i + 1).ToString());
        }
        ui.gridWidthSelect.AddOptions(gridOptions);
        ui.gridWidthSelect.value = 7;
        ui.gridLengthSelect.AddOptions(gridOptions);
        ui.gridLengthSelect.value = 7;
    }

    public void CreateGrid(int gridWidth, int gridLength)
    {
        GameObject.Destroy(tiles);
        float tileWidth = tilePrefab.transform.localScale.x;
        float tileHeight = tilePrefab.transform.localScale.y;
        float tileLength = tilePrefab.transform.localScale.z;
        tiles = new GameObject("Tiles");
        tiles.transform.localPosition = 
            new Vector3(-0.5f * ((gridWidth - 1) * tileWidth), 0.0f, -0.5f * ((gridLength - 1) * tileLength));

        Vector3 localPosition;
        Color color;
        for (int j = 0; j < gridLength; j++)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                localPosition = new Vector3(i * tileWidth, -0.5f * tileHeight, j * tileLength);
                GameObject newTile = Instantiate(tilePrefab, tiles.transform);
                newTile.transform.localPosition = localPosition;
                color = ((i + j) % 2 == 0) ? darkColor : lightColor;
                newTile.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }

        FindObjectOfType<CameraController>().ResetCamera();
    }
}
