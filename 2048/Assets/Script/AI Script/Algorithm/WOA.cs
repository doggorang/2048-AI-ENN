using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WOA
{
    public List<Individual> Population = new List<Individual>();
    private List<Individual> NewPopulation = new List<Individual>();
    public int populationSize, generation;
    private bool isTree; // aku simpen isTree supaya pas initialize if nya engga berat jadi langsung akses bool
    private int IndSize; // ukuran individu karen kalo tree dan NN ukuran nya beda
    private int numLayer, numNeuron;
    public ArchitectureOption architecture;

    private float a = 2.0f; // a disini artinya control search spread digunakan pada rumus metode" lain default: 2.0
    private float b = 0.5f; // b disini artinya controls spiral default: 0.5 ++
    private float a_step; // didapat dari hasil perhitungan yaitu step pengurangan a dari 2 sampai 0
    private float ngens = 20.0f; // number of max generations

    public WOA(int populationSize, ArchitectureOption architecture, int layer = 0, int neuron = 0)
    {
        generation = 0;
        a_step = a / ngens;
        numLayer = layer; numNeuron = neuron;
        this.populationSize = populationSize;
        this.architecture = architecture;
        for (int i = 0; i < populationSize; i++)
        {
            List<float> tempW = new List<float>();
            // IndSize ini ukuran individu kalau tree pasti 6 kalau NN harus di itung dulu
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

    public void Optimize()
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

        // sorting population
        Population.Sort(AIController.SortFunc);
        Individual best = Population[0];
        NewPopulation.Add(best.InitialiseCopy(numLayer, numNeuron));
        foreach (Individual sol in Population)
        {
            float A = Compute_A(); // get capital A menggunakan equation 3
            // probability 50% antara mau attack atau mendekati prey
            if (Random.value < 0.5f)
            {
                float C = 2.0f * Random.value; // Equation  2.4
                if (A < 1.0f)
                {
                    // if hasil kalkulasi A < 1 maka individu dekat dengan prey maka jalankan algoritma encircle
                    Encircle(sol, best, A, C);
                }
                else
                {
                    // else maka individu jauh sehingga perlu random individu sebagai reference prey lalu jalankan algoritma search
                    int rnd = Random.Range(0, populationSize);
                    Individual rndSol = Population[rnd];
                    Search(sol, rndSol, A, C);
                }
            }
            else
            {
                Attack(sol, best); // jalankan algoritma attack
            }
        }
        Population.Clear();
        foreach (Individual p in NewPopulation)
        {
            Population.Add(p.InitialiseCopy(numLayer, numNeuron));
        }
        generation++;
        a -= a_step;
        NewPopulation.Clear();
    }

    private void Encircle(Individual i, Individual best, float A, float C)
    {
        List<float> newIndWeight = new List<float>();
        for (int j = 0; j < IndSize; j++)
        {
            // Equation  2.1
            float D = Mathf.Abs(C * best.Weights[j] - i.Weights[j]);

            // Equation  2.2
            newIndWeight.Add(best.Weights[j] - A * D);
        }
        NewPopulation.Add(new Individual(newIndWeight, architecture, numLayer, numNeuron));
    }
    private void Search(Individual i, Individual rndSol, float A, float C)
    {
        List<float> newIndWeight = new List<float>();
        for (int j = 0; j < IndSize; j++)
        {
            // Equation  2.7
            float D = Mathf.Abs(C * rndSol.Weights[j] - i.Weights[j]);

            // Equation  2.8
            newIndWeight.Add(rndSol.Weights[j] - A * D);
        }
        NewPopulation.Add(new Individual(newIndWeight, architecture, numLayer, numNeuron));
    }
    private void Attack(Individual i, Individual best)
    {
        List<float> newIndWeight = new List<float>();
        // Equation  2.5
        float L = Random.Range(-1f, 1f);
        for (int j = 0; j < IndSize; j++)
        {
            float D = Mathf.Abs(i.Weights[j] - best.Weights[j]);
            newIndWeight.Add(D * Mathf.Exp(b * L) * Mathf.Cos(L * 2.0f * Mathf.PI) + best.Weights[j]);
        }
        NewPopulation.Add(new Individual(newIndWeight, architecture, numLayer, numNeuron));
    }

    private float Compute_A()
    {
        // Equation 2.3
        float temp_A = 2.0f * a * Random.value - a;
        float ret_A = Mathf.Abs(temp_A);
        return ret_A;
    }
}
