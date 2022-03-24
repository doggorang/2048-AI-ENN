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
    public int layer = 0;
    public int neuron = 0;
    public ArchitectureOption ao;
    public AlgorithmOption algo;
    public Individual(List<float> weights, ArchitectureOption ao, AlgorithmOption algo, int layer, int neuron)
    {
        Weights = weights; this.ao = ao; this.algo = algo;
        this.layer = layer; this.neuron = neuron;
        if (ao == ArchitectureOption.NN)
        {
            nn = new NN(weights, layer, neuron);
        }
    }

    public Individual InitialiseCopy(int layer = 0, int neuron = 0)
    {
        Individual i = new Individual(this.Weights, this.ao, this.algo, layer, neuron);
        return i;
    }
}
