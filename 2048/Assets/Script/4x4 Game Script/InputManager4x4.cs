using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    // urutan move direction Left, Up, Right, Down
    Left, Up, Right, Down
}

public class InputManager4x4 : MonoBehaviour
{
    private GameScript4x4 gm;

    private void Awake()
    {
        gm = GameObject.FindObjectOfType<GameScript4x4>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.State == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                gm.Move(MoveDirection.Right);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gm.Move(MoveDirection.Left);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gm.Move(MoveDirection.Up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gm.Move(MoveDirection.Down);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                //float[] tempWeights = new float[6] { 1, 1, 1, 1, 1, 1 };
                //Debug.Log(gm.TreeSimulation(tempWeights));
            }
        }
    }
}
