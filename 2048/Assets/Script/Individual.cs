using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual
{
    public int HighestTile = 0;
    public int Score = 0;
    public float Fitness = 0;
    public float GameTime = 0;
    public List<float> Weights = new List<float>();
    public NN nn;
    public A_Tree tree;
    public Individual(List<float> weights, ArchitectureOption ao, int layer=0, int neuron=0)
    {
        Weights = weights;
        if (ao == ArchitectureOption.NN)
        {

        }
        else
        {

        }
    }
}
