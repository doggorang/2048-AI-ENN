using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Tile[,] AllTiles = new Tile[4, 4];
    private List<Tile[]> columns = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();
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
        columns.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 0], AllTiles[2, 0], AllTiles[3, 0] });
        columns.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 1], AllTiles[2, 1], AllTiles[3, 1] });
        columns.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 2], AllTiles[2, 2], AllTiles[3, 2] });
        columns.Add(new Tile[] { AllTiles[0, 3], AllTiles[1, 3], AllTiles[2, 3], AllTiles[3, 3] });

        rows.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3] });
        rows.Add(new Tile[] { AllTiles[1, 0], AllTiles[1, 1], AllTiles[1, 2], AllTiles[1, 3] });
        rows.Add(new Tile[] { AllTiles[2, 0], AllTiles[2, 1], AllTiles[2, 2], AllTiles[2, 3] });
        rows.Add(new Tile[] { AllTiles[3, 0], AllTiles[3, 1], AllTiles[3, 2], AllTiles[3, 3] });
    }

    bool MakeOneMoveDownIndex(Tile[] LineOfTiles)
    {
        for (int i = 0; i < LineOfTiles.Length-1; i++)
        {
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i+1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;
                return true;
            }
        }
        return false;
    }
    bool MakeOneMoveUpIndex(Tile[] LineOfTiles)
    {
        for (int i = LineOfTiles.Length-1; i > 0; i--)
        {
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i-1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
                LineOfTiles[i - 1].Number = 0;
                return true;
            }
        }
        return false;
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
        for (int i = 0; i < rows.Count; i++)
        {
            switch (md)
            {
                case MoveDirection.Left:
                    while (MakeOneMoveDownIndex(rows[i])) { }
                    break;
                case MoveDirection.Right:
                    while (MakeOneMoveUpIndex(rows[i])) { }
                    break;
                case MoveDirection.Up:
                    while (MakeOneMoveDownIndex(columns[i])) { }
                    break;
                case MoveDirection.Down:
                    while (MakeOneMoveUpIndex(columns[i])) { }
                    break;
                default:
                    break;
            }
        }
    }
}
