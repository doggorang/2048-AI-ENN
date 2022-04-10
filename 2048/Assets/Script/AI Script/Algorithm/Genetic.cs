using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genetic
{
    public List<Individual> Population = new List<Individual>();
    private List<Individual> Parents = new List<Individual>();
    private List<Individual> Children = new List<Individual>();
    private List<Individual> NewPopulation = new List<Individual>();
    public int populationSize, generation, mapSize;
    private float mutationRate = 0.055f;
    private bool isTree; // aku simpen isTree supaya pas initialize if nya engga berat jadi langsung akses bool
    private int IndSize; // ukuran individu karen kalo tree dan NN ukuran nya beda
    private int numLayer, numNeuron;
    public ArchitectureOption architecture;

    int bitCount = sizeof(float) * 8;

    public Genetic(int populationSize, ArchitectureOption architecture, int mapSize, int layer, int neuron)
    {
        generation = 0;
        numLayer = layer; numNeuron = neuron;
        this.mapSize = mapSize;
        this.populationSize = populationSize;
        this.architecture = architecture;
        for (int i = 0; i < populationSize; i++)
        {
            List<float> tempW = new List<float>();
            // IndSize ini ukuran individu kalau tree pasti 6 kalau NN harus di itung dulu
            if (architecture == ArchitectureOption.Tree)
            {
                isTree = true; IndSize = 6; // ini 6 input layer
                //isTree = true; IndSize = mapSize* mapSize; // ini coba input layer map size
            }
            else
            {
                isTree = false;
                // (layer + 1)->bias + (6 * neuron)->input layer + ((layer-1)*neuron*neuron)->hidden layer + (4 * neuron)->output layer
                IndSize = (layer + 1) + (6 * neuron) + ((layer - 1) * neuron * neuron) + (4 * neuron); // ini 6 input layer
                //IndSize = (layer + 1) + (mapSize * mapSize * neuron) + ((layer - 1) * neuron * neuron) + (4 * neuron); // ini coba input layer map size
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
            Population.Add(new Individual(tempW, architecture, AlgorithmOption.Genetic, layer, neuron));
        }
    }

    public void RePopulate()
    {
        // selection hitung fitness lalu dapetin parents yaitu top 20% best
        Selection();
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
    private void Selection()
    {
        int HighScore = PlayerPrefs.GetInt($"HighScore{mapSize}");
        // calculate every individual's fitness
        for (int i = 0; i < Population.Count; i++)
        {
            // calculate fitness highest tile saja karena saat endgame biasanya sudah berantakan jadi second highest tile dll pindah"
            float temp = (((float)Population[i].HighestTile / (float)2048) + ((float)Population[i].Score / (float)HighScore)) / 2;
            Population[i].Fitness = temp;
        }

        // print population urut dengan fitness
        AIController.PrintPopulation(Population, generation, mapSize);
        // sorting population untuk dapat diambil 20% top nya sebagai parent
        Population.Sort(AIController.SortFunc);
        for (int i = 0; i < populationSize/5; i++)
        {
            Parents.Add(Population[i].InitialiseCopy(numLayer, numNeuron));
        }
    }
    private void Crossover()
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
                string parent1Binary = ToBinaryString(Population[parent1].Weights[j]);
                string parent2Binary = ToBinaryString(Population[parent2].Weights[j]);
                string newBinaryW = "";
                for (int k = 0; k < bitCount; k++)
                {
                    if (Random.value < 0.5) // kalau kurang dari 0.5 maka ambil dari parent1 else parent 2
                        newBinaryW += parent1Binary[k];
                    else
                        newBinaryW += parent2Binary[k];
                }
                newIndWeight.Add(FromBinaryString(newBinaryW));
            }
            Children.Add(new Individual(newIndWeight, architecture, AlgorithmOption.Genetic, numLayer, numNeuron));
        }
    }
    private void Mutation()
    {
        foreach (Individual Ind in Children)
        {
            // kalau hasil random < dari mutationRate maka di mutasi akan dilakukan
            if (Random.value < mutationRate)
            {
                // pakai cara swap mutation
                int point1, point2;
                point1 = Random.Range(0, IndSize);
                do
                {
                    point2 = Random.Range(0, IndSize);
                } while (point1 == point2);
                float temp = Ind.Weights[point1];
                Ind.Weights[point1] = Ind.Weights[point2];
                Ind.Weights[point2] = temp;
            }
        }
    }
    private string ToBinaryString(float value)
    {
        int intValue = System.BitConverter.ToInt32(System.BitConverter.GetBytes(value), 0);
        return System.Convert.ToString(intValue, 2).PadLeft(bitCount, '0');
    }

    private float FromBinaryString(string bstra)
    {
        int intValue = System.Convert.ToInt32(bstra, 2);
        return System.BitConverter.ToSingle(System.BitConverter.GetBytes(intValue), 0);
    }
}
