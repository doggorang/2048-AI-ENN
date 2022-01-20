using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Playing,
    GameOver,
    WaitingForMoveToEnd
}

public class GameScript4x4 : MonoBehaviour
{
    public GameState State;
    [Range(0,2f)]
    public float delay;
    private bool moveMade;
    // bolean to check if move is done because it has delay so it doesnt move around
    private bool[] lineMoveComplete = new bool[4] { true, true, true, true };

    public Text TextDescriptionAlgorithm, TextDescriptionArchitecture;
    public Text GameOverText;
    public GameObject GameOverPanel;
    private Tile[,] AllTiles = new Tile[4, 4];
    private List<Tile[]> columns = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();
    private List<Tile> EmptyTiles = new List<Tile>();

    // variable untuk simpan 6 input layer
    public int HighestTile = 0;
    public int SequenceTile = 0;
    public bool IsHighestTileCorner = false;
    public int SequenceMerge = 0;
    public int CountSmallTile = 0;
    public bool IsHighestTileDense = false;

    // Start is called before the first frame update
    void Start()
    {
        TextDescriptionAlgorithm.text = "Algorithm  - <b>"+ AIController.algorithm + "</b>";
        TextDescriptionArchitecture.text = "Architecture - <b>"+ AIController.architecture+ "</b>";
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
        Generate(); Generate();
    }

    private void GameOver(string text)
    {
        GameOverText.text = text;
        GameOverPanel.SetActive(true);
    }

    bool CanMove()
    {
        // if there is an empty tile that means you can still move
        if (EmptyTiles.Count > 0)
        {
            return true;
        }
        else
        {
            // if there are no move check if there are any tile that can merge
            // check columns
            for (int i = 0; i < columns.Count; i++)
                for (int j = 0; j < rows.Count-1; j++)
                    if (AllTiles[j,i].Number == AllTiles[j+1, i].Number)
                        return true;
            // check rows
            for (int i = 0; i < rows.Count; i++)
                for (int j = 0; j < columns.Count-1; j++)
                    if (AllTiles[i, j].Number == AllTiles[i, j+1].Number)
                        return true;
        }
        return false;
    }

    bool MakeOneMoveDownIndex(Tile[] LineOfTiles)
    {
        for (int i = 0; i < LineOfTiles.Length-1; i++)
        {
            // check 1 block away if this tile empty and next tile has number then swicth place
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i+1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;
                return true;
            }
            // merge tile if 2 colliding tile has the same number also check if the tile hasn't merge because only can merge once
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i+1].Number && !LineOfTiles[i].mergeThisTurn && !LineOfTiles[i+1].mergeThisTurn)
            {
                LineOfTiles[i].Number *= 2;
                LineOfTiles[i].mergeThisTurn = true;
                LineOfTiles[i + 1].Number = 0;
                LineOfTiles[i].PlayMergeAnimation();
                ScoreTracker4x4.Instance.Score += LineOfTiles[i].Number;
                if (LineOfTiles[i].Number == 2048)
                {
                    GameOver("You Win");
                }
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
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i - 1].Number && !LineOfTiles[i].mergeThisTurn && !LineOfTiles[i - 1].mergeThisTurn)
            {
                LineOfTiles[i].Number *= 2;
                LineOfTiles[i].mergeThisTurn = true;
                LineOfTiles[i - 1].Number = 0;
                LineOfTiles[i].PlayMergeAnimation();
                ScoreTracker4x4.Instance.Score += LineOfTiles[i].Number;
                if (LineOfTiles[i].Number == 2048)
                {
                    GameOver("You Win");
                }
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

            EmptyTiles[idxnewnumber].PlayAppearAnimation();
            EmptyTiles.RemoveAt(idxnewnumber);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ResetMergedFlags()
    {
        foreach (Tile t in AllTiles)
            t.mergeThisTurn = false;
    }

    private void UpdateEmptyTiles()
    {
        EmptyTiles.Clear();
        foreach (Tile t in AllTiles)
            if (t.Number == 0)
                EmptyTiles.Add(t);
    }

    public void Move(MoveDirection md)
    {
        moveMade = false;
        ResetMergedFlags();
        if (delay > 0)
        {
            StartCoroutine(MoveCaroutine(md));
        }
        else
        {
            for (int i = 0; i < rows.Count; i++)
            {
                switch (md)
                {
                    case MoveDirection.Left:
                        while (MakeOneMoveDownIndex(rows[i]))
                            moveMade = true;
                        break;
                    case MoveDirection.Right:
                        while (MakeOneMoveUpIndex(rows[i]))
                            moveMade = true;
                        break;
                    case MoveDirection.Up:
                        while (MakeOneMoveDownIndex(columns[i]))
                            moveMade = true;
                        break;
                    case MoveDirection.Down:
                        while (MakeOneMoveUpIndex(columns[i]))
                            moveMade = true;
                        break;
                    default:
                        break;
                }
            }
            if (moveMade)
            {
                UpdateEmptyTiles();
                Generate();
                if (!CanMove())
                {
                    GameOver("You Lose");
                }
            }
        }
    }

    IEnumerator MoveOneLineUpIndexCoroutine(Tile[] line, int index)
    {
        lineMoveComplete[index] = false;
        while (MakeOneMoveUpIndex(line))
        {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }
    IEnumerator MoveOneLineDownIndexCoroutine(Tile[] line, int index)
    {
        lineMoveComplete[index] = false;
        while (MakeOneMoveDownIndex(line))
        {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }

    IEnumerator MoveCaroutine(MoveDirection md)
    {
        State = GameState.WaitingForMoveToEnd;
        switch (md)
        {
            case MoveDirection.Left:
                for (int i = 0; i < rows.Count; i++)
                {
                    StartCoroutine(MoveOneLineDownIndexCoroutine(rows[i],i));
                }
                break;
            case MoveDirection.Right:
                for (int i = 0; i < rows.Count; i++)
                {
                    StartCoroutine(MoveOneLineUpIndexCoroutine(rows[i], i));
                }
                break;
            case MoveDirection.Up:
                for (int i = 0; i < columns.Count; i++)
                {
                    StartCoroutine(MoveOneLineDownIndexCoroutine(columns[i], i));
                }
                break;
            case MoveDirection.Down:
                for (int i = 0; i < columns.Count; i++)
                {
                    StartCoroutine(MoveOneLineUpIndexCoroutine(columns[i], i));
                }
                break;
            default:
                break;
        }

        while (!(lineMoveComplete[0] && lineMoveComplete[1] && lineMoveComplete[2] && lineMoveComplete[3]))
        {
            yield return null;
        }

        if (moveMade)
        {
            UpdateEmptyTiles();
            Generate();
            if (!CanMove())
            {
                GameOver("You Lose");
            }
        }
        State = GameState.Playing;
        StopAllCoroutines();
    }
}
