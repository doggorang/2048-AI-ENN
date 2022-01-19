using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour
{
    public int HighestTile = 0;
    public int Score = 0;
    public float Fitness = 0;
    public float[] Weights = new float[6];
    public Individual(float[] weights)
    {
        Weights = weights;
    }

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
