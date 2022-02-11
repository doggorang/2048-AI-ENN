using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Genetic
{
    public GameScript4x4 controller;
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

    public Genetic(int populationSize, ArchitectureOption architecture, int layer = 0, int neuron = 0)
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
                IndSize = (layer + 1)+ (6 * neuron)+ ((layer - 1) * neuron * neuron)+ (4 * neuron);
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

    public void RePopulate()
    {
        // selection hitung fitness lalu dapetin parents yaitu top 20% best
        Selection();
        PrintPopulation(architecture + "");
        // crossover akan menghasil kan 80% untuk jadi children
        Crossover();
        // children yang tercipta akan dilewatkan mutation
        Mutation();
        // insert parent and childer to current population
        Population.Clear();
        foreach (Individual p in Parents)
        {
            Population.Add(p.InitialiseCopy(numLayer, numNeuron));
        }
        foreach (Individual c in Children)
        {
            Population.Add(c.InitialiseCopy(numLayer, numNeuron));
        }
        generation++;
        // lalu list of parent dan children dan new population di kosongkan
        Parents.Clear(); Children.Clear(); NewPopulation.Clear();
    }
    public void Selection()
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

        // sorting population untuk dapat diambil 20% top nya sebagai parent
        Population.Sort(SortFunc);
        for (int i = 0; i < populationSize/5; i++)
        {
            Parents.Add(Population[i].InitialiseCopy(numLayer, numNeuron));
        }
    }
    public void Crossover()
    {
        // jumlah children adalah 80% total populasi
        int numChildren = populationSize * 4 / 5;
        int numParents = populationSize - numChildren;
        for (int i = 0; i < numChildren; i++)
        {
            // cari random parent
            int parent1, parent2;
            parent1 = Random.Range(0, numParents);
            do
            {
                parent2 = Random.Range(0, numParents);
            } while (parent1 == parent2);

            // weight untuk individu baru yaitu random 50/50 apakah ngambil dari parent 1 atau 2
            List<float> newIndWeight = new List<float>();
            for (int j = 0; j < IndSize; j++)
            {
                int choosenParent;
                if (Random.value < 0.5) // kalau kurang dari 0.5 maka ambil dari parent1 else parent 2
                    choosenParent = parent1;
                else
                    choosenParent = parent2;
                newIndWeight.Add(Population[choosenParent].Weights[j]);
            }
            Children.Add(new Individual(newIndWeight, architecture, numLayer, numNeuron));
        }
    }
    public void Mutation()
    {
        foreach (Individual Ind in Children)
        {
            // pakai cara swap mutation
            int point1, point2;
            point1 = Random.Range(0, IndSize);
            do
            {
                point2 = Random.Range(0, IndSize);
            } while (point1 == point2);
            // kalau hasil random < dari mutationRate maka di mutasi akan dilakukan
            if (Random.value < mutationRate)
            {
                float temp = Ind.Weights[point1];
                Ind.Weights[point1] = Ind.Weights[point2];
                Ind.Weights[point2] = temp;
            }
        }
    }

    public void PrintPopulation(string Architecture)
    {
        string path = Application.dataPath + "/Log/Genetic " + Architecture + ".txt";
        string content = "";
        for (int i = 0; i < Population.Count; i++)
        {
            content += "Generation: " + generation + " Population: " + i + " Fitness: "+ Population[i].Fitness + "\nWeight: [ ";
            foreach (float w in Population[i].Weights)
            {
                content += w + ", ";
            }
            content+="]\n";
        }
        content += "\n";
        if (!File.Exists(path))
            File.WriteAllText(path, content);
        else
            File.AppendAllText(path, content);
    }

    private int SortFunc(Individual a, Individual b)
    {
        if (a.Fitness < b.Fitness)
        {
            return -1;
        }
        else if (a.Fitness > b.Fitness)
        {
            return 1;
        }
        return 0;
    }
}
