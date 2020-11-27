using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Tile currentTile;
    [SerializeField] public Hero hero;

    // Start is called before the first frame update
    void Awake()
    {
        if (currentTile == null)
        {
            currentTile = FindObjectOfType<Tile>();
        }
        MoveToCurrentTile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToCurrentTile()
    {
        if (currentTile != null)
        {
            transform.position = currentTile.transform.position;
            transform.position += Vector3.up * transform.localScale.y;
        }
    }
}
