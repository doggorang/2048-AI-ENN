using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFO
{
    public List<Individual> Population = new List<Individual>();
    private List<Individual> Parents = new List<Individual>();
    private List<Individual> Children = new List<Individual>();
    private List<Individual> NewPopulation = new List<Individual>();
    public int populationSize, generation;
    private float mutationRate = 0.055f;
    private bool isTree; // aku simpen isTree supaya pas initialize if nya engga berat jadi langsung akses bool
    private int IndSize; // ukuran individu karen kalo tree dan NN ukuran nya beda
    private int numLayer, numNeuron;
    public ArchitectureOption architecture;

    public MFO(int populationSize, ArchitectureOption architecture, int layer = 0, int neuron = 0)
    {
        generation = 0;
        numLayer = layer; numNeuron = neuron;
        this.populationSize = populationSize;
        this.architecture = architecture;
        for (int i = 0; i < populationSize; i++)
        {
            List<float> tempW = new List<float>();
            // ctrSize ini ukuran individu kalau tree pasti 6 kalau NN harus di itung dulu
            if (architecture == ArchitectureOption.Tree)
            {
                isTree = true; IndSize = 6;
            }
            else
            {
                // (layer + 1)->bias + (6 * neuron)->input layer + ((layer-1)*neuron*neuron)->hidden layer + (4 * neuron)->output layer
                IndSize = (layer + 1) + (6 * neuron) + ((layer - 1) * neuron * neuron) + (4 * neuron);
                isTree = false;
            }
            for (int j = 0; j < IndSize; j++)
            {
                float rndVal;
                // kalau tree random nya cuma 0-1 kalau NN maka random range dari -1 sampe 1
                if (isTree)
                    rndVal = Random.value;
                else
                    rndVal = Random.Range(-1f, 1f);
                tempW.Add(rndVal);
            }
            Population.Add(new Individual(tempW, architecture, layer, neuron));
        }
    }

}
