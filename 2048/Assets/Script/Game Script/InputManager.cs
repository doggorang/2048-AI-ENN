using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameScript gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameScript>();
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
                //List<float> tempWeights = new List<float>() { 1,1,1,1,1,1 };
                //Debug.Log(gm.TreeSimulation(tempWeights));
            }
        }
    }
}
