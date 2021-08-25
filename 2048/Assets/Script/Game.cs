using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Tile[,] AllTiles = new Tile[4, 4];
    private List<Tile> EmptyTiles = new List<Tile>();
    // Start is called before the first frame update
    void Start()
    {
        Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile>();
        foreach (Tile t in AllTilesOneDim)
        {
            t.Number = 0;
            AllTiles[t.indRow, t.indCol] = t;
            EmptyTiles.Add(t);
        }
    }

    void Generate()
    {
        if (EmptyTiles.Count > 0)
        {
            int idxnewnumber = Random.Range(0, EmptyTiles.Count);
            int randomnum = Random.Range(0,10);
            if (randomnum == 0)
                EmptyTiles[idxnewnumber].Number = 4;
            else
                EmptyTiles[idxnewnumber].Number = 2;        
            EmptyTiles.RemoveAt(idxnewnumber);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
    }

    public void Move(MoveDirection md)
    {
        
    }
}
