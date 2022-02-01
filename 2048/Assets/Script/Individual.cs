using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual
{
    public int HighestTile = 0;
    public int Score = 0;
    public float Fitness = 0;
    public float GameTime = 0;
    public float[] Weights = new float[6];
    public Individual(float[] weights)
    {
        Weights = weights;
    }
}
