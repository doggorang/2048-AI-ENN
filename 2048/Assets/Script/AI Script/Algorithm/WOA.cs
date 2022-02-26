using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WOA
{
    public List<Individual> Population = new List<Individual>();
    public int populationSize, generation;
    private bool isTree; // aku simpen isTree supaya pas initialize if nya engga berat jadi langsung akses bool
    private int IndSize; // ukuran individu karen kalo tree dan NN ukuran nya beda
    private int numLayer, numNeuron;
    public ArchitectureOption architecture;

    private float a = 2.0f, b = 0.5f, a_step, ngens = 20.0f;

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
        foreach (Individual sol in Population)
        {
            a -= a_step;
            float A = Compute_A(); // get capital A menggunakan equation 3
            // probability 50% antara mau attack atau mendekati prey
            if (Random.value < 0.5f)
            {
                
                if (A < 1.0f)
                {
                    // if hasil kalkulasi A < 1 maka individu dekat dengan prey maka jalankan algoritma encircle
                    Encircle(sol, best, A);
                }
                else
                {
                    // else maka individu jauh sehingga perlu random individu sebagai reference prey lalu jalankan algoritma search
                    int rnd = Random.Range(0, populationSize);
                    Individual rndSol = Population[rnd];
                    Search(sol, rndSol, A);
                }
            }
            else
            {
                Attack(sol, best); // jalankan algoritma attack
            }
        }
    }

    private void Encircle(Individual i, Individual best, float A)
    {

    }
    private void Search(Individual i, Individual rndSol, float A)
    {

    }
    private void Attack(Individual i, Individual best)
    {
        // Equation  7
        
        //D = np.linalg.norm(best_sol - sol)
        //L = np.random.uniform(-1.0, 1.0, size = 2)
        //return np.multiply(np.multiply(D, np.exp(self._b * L)), np.cos(2.0 * np.pi * L)) + best_sol
    }

    private float Compute_A()
    {
        // Equation 3
        float temp_A = 2.0f * a * Random.value - a;
        float ret_A = Mathf.Abs(temp_A);
        return ret_A;
    }
}
