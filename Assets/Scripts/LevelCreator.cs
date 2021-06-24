using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Color lightColor;
    public Color darkColor;
    private GameObject tiles;

    public int minSideLength = 1;
    public int maxSideLength = 64;

    public int standardWidth = 15;
    public int standardLength = 15;

    private void Awake()
    {
        UIManager ui = GetComponent<UIManager>();
        List<string> gridOptions = new List<string>();
        for (int i = minSideLength; i <= maxSideLength; i++)
        {
            gridOptions.Add((i).ToString());
        }
        ui.gridWidthSelect.AddOptions(gridOptions);
        ui.gridWidthSelect.value = standardWidth - 1;
        ui.gridLengthSelect.AddOptions(gridOptions);
        ui.gridLengthSelect.value = standardLength - 1;

        CreateGrid(standardWidth, standardLength);
    }

    public void CreateGrid(int gridWidth, int gridLength)
    {
        if (gridWidth < minSideLength)
            gridWidth = minSideLength;
        if (gridWidth > maxSideLength)
            gridWidth = maxSideLength;
        if (gridLength < minSideLength)
            gridLength = minSideLength;
        if (gridLength > maxSideLength)
            gridLength = maxSideLength;

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
