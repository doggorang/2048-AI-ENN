using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Genetic
{
    public List<Individual> Population = new List<Individual>();
    public int populationSize;
    public int generation;

    private Individual fittest, secondFittest;
    private Individual unfittest, secondUnfittest;
    private int ctrUnfittest, ctrSecondUnfittest;
    public Genetic(int populationSize)
    {
        generation = 0;
        this.populationSize = populationSize;
        for (int i = 0; i < populationSize; i++)
        {
            float[] tempW = new float[6];
            for (int j = 0; j < 6; j++)
            {
                tempW[j] = Random.value;
            }
            Population.Add(new Individual(tempW));
        }
        unfittest = new Individual(new float[] { 0, 0, 0, 0, 0, 0 });
        fittest = new Individual(new float[] { 0, 0, 0, 0, 0, 0 });
        secondUnfittest = new Individual(new float[] { 0, 0, 0, 0, 0, 0 });
        secondFittest = new Individual(new float[] { 0, 0, 0, 0, 0, 0 });
    }

    public void RePopulate(string Architecture)
    {
        // selection select sum to be parent more than 2 preffearble
        Selection();
        PrintPopulation(Architecture);
        // crossover sejumlah sisan total pupulation selain parent -> total population - parent.count
        // setiap offspring akan merandom untuk memilih 2 parent dari array of parent
        // untuk weight random 50% antara ngambil dari parent 1 atau 2
        Crossover();
        // mutation untuk setiap offspring
        Mutation();
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
            Population[i].Fitness = ((Population[i].HighestTile / 2048) + (Population[i].Score / sumScore)) / 2;
            // find parent with most fit and second fit
            if (Population[i].Fitness > fittest.Fitness)
            {
                fittest.Weights = Population[i].Weights;
            }
            else if (Population[i].Fitness > secondFittest.Fitness)
            {
                secondFittest.Weights = Population[i].Weights;
            }
            // find position of the most unfit and second unfit to be replace by offspring after mutation
            if (Population[i].Fitness < unfittest.Fitness)
            {
                unfittest.Weights = Population[i].Weights;
                ctrUnfittest = i;
            }
            else if (Population[i].Fitness > secondUnfittest.Fitness)
            {
                secondUnfittest.Weights = Population[i].Weights;
                ctrSecondUnfittest = i;
            }
        }
        // reset unfittest
        unfittest.Fitness = float.MaxValue;
        secondUnfittest.Fitness = float.MaxValue;
    }
    public void Crossover()
    {
        // offspring disimpan di fittest dan second fittest karena sudah tidak dipakai
        int crossoverPoint = Random.Range(0, 6);
        for (int i = 0; i < crossoverPoint; i++)
        {
            float temp = fittest.Weights[i];
            fittest.Weights[i] = secondFittest.Weights[i];
            secondFittest.Weights[i] = temp;
        }
    }
    public void Mutation()
    {
        // use swap mutation
        int point1, point2;
        point1 = Random.Range(0, 6);
        do
        {
            point2 = Random.Range(0, 6);
        } while (point1 == point2);
        // kalau hasil random < 0.05 maka di mutasi
        if (Random.value < 0.05)
        {
            float temp = fittest.Weights[point1];
            fittest.Weights[point1] = fittest.Weights[point2];
            fittest.Weights[point2] = temp;
        }
        if (Random.value < 0.05)
        {
            float temp = secondFittest.Weights[point1];
            secondFittest.Weights[point1] = secondFittest.Weights[point2];
            secondFittest.Weights[point2] = temp;
        }
        // insert offspring ke population
        Population[ctrUnfittest].Weights = fittest.Weights;
        Population[ctrSecondUnfittest].Weights = secondFittest.Weights;
        generation++;
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
}
