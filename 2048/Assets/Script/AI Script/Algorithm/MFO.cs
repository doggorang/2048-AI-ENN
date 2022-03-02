using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MFO
{
    public List<Individual> Population = new List<Individual>();
    public List<Individual> PreviousPopulation = new List<Individual>();
    private List<Individual> BestFlames = new List<Individual>();
    public int populationSize, generation;
    private bool isTree; // aku simpen isTree supaya pas initialize if nya engga berat jadi langsung akses bool
    private int IndSize; // ukuran individu karen kalo tree dan NN ukuran nya beda
    private int numLayer, numNeuron;
    public ArchitectureOption architecture;

    private float a, FlameNo;

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

    public void UpdateMothPosition()
    {
        CalculateFitness();
        GetBestFlame();

        // mendapatkan previous population yaitu populasi sebelum posisi dirubah
        PreviousPopulation.Clear();
        foreach (Individual Ind in Population)
        {
            PreviousPopulation.Add(Ind.InitialiseCopy(numLayer, numNeuron));
        }
        // Update the position of best flame obtained so far

        // a linearly dicreases from -1 to -2 to calculate t in Eq. (3.12)
        //a = -1.0f + (float)(iteration + 1) * (-1.0f / (float)maxIterations_);
        //FlameNo = round((searchAgentsCount_ - 1) - (iteration + 1) * ((float)(searchAgentsCount_ - 1) / (float)maxIterations_));

        //updatePosition(a, FlameNo);

        //convergenceCurve_[iteration] = bestFlames_->get(0)->getObjective(0);
    }
    private void CalculateFitness()
    {
        // get the sum score to be used in fitness calculation
        int sumScore = 0;
        foreach (Individual i in Population)
        {
            sumScore += i.Score;
        }
        // calculate every individual's fitness
        for (int i = 0; i < Population.Count; i++)
        {
            // calculate fitness highest tile saja karena saat endgame biasanya sudah berantakan jadi second highest tile dll pindah"
            float temp = (((float)Population[i].HighestTile / (float)2048) + ((float)Population[i].Score / (float)sumScore)) / 2;
            Population[i].Fitness = temp;
        }
    }
    private void GetBestFlame()
    {
        if (generation == 0)
        {
            foreach (Individual Ind in Population)
            {
                BestFlames.Add(Ind.InitialiseCopy(numLayer, numNeuron));
            }
            // jika generasi 1 maka best flame ambil dari populasi saja
            BestFlames.Sort(AIController.SortFunc);
        }
        else
        {
            // else best flame ambil dari gabungan populasi lama dan best flame sebelumnya
            List<Individual> TempJoinPopulation = new List<Individual>();
            foreach (Individual Ind in BestFlames)
            {
                TempJoinPopulation.Add(Ind.InitialiseCopy(numLayer, numNeuron));
            }
            foreach (Individual Ind in PreviousPopulation)
            {
                TempJoinPopulation.Add(Ind.InitialiseCopy(numLayer, numNeuron));
            }
            TempJoinPopulation.Sort(AIController.SortFunc);

            BestFlames.Clear();
            for (int i = 0; i < populationSize; i++)
            {
                BestFlames.Add(TempJoinPopulation[i].InitialiseCopy(numLayer, numNeuron));
            }
        }
    }

    public void PrintPopulation(string Architecture)
    {
        string path = Application.dataPath + "/Log/WOA " + Architecture + ".txt";
        string content = "";
        for (int i = 0; i < Population.Count; i++)
        {
            content += "Generation: " + generation + " Population: " + i + " Fitness: " + Population[i].Fitness + "\nWeight: [ ";
            foreach (float w in Population[i].Weights)
            {
                content += w + ", ";
            }
            content += "]\n";
        }
        content += "\n";
        if (!File.Exists(path))
            File.WriteAllText(path, content);
        else
            File.AppendAllText(path, content);
    }
}
