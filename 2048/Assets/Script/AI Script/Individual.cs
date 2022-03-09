using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Individual
{
    public int HighestTile = 0;
    public int Score = 0;
    public float Fitness = 0;
    public float GameTime = 0;
    public List<float> Weights = new List<float>();
    public NN nn;
    private ArchitectureOption ao;
    public Individual(List<float> weights, ArchitectureOption ao, int layer = 0, int neuron = 0)
    {
        Weights = weights; this.ao = ao;
        if (ao == ArchitectureOption.NN)
        {
            nn = new NN(weights, layer, neuron);
        }
    }

    public Individual InitialiseCopy(int layer = 0, int neuron = 0)
    {
        Individual i = new Individual(this.Weights, this.ao, layer, neuron);
        return i;
    }
}
