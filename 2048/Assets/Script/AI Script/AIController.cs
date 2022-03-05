using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum AlgorithmOption
{
    Genetic, WOA, MFO
}
public enum ArchitectureOption
{
    NN, Tree
}

public class AIController : MonoBehaviour
{
    public static AlgorithmOption algorithm;
    public static ArchitectureOption architecture;
    public static void SwitchCaseAlgorithm(string value)
    {
        switch (value)
        {
            case "Genetic":
                algorithm = AlgorithmOption.Genetic;
                break;
            case "WOA":
                algorithm = AlgorithmOption.WOA;
                break;
            case "MFO":
                algorithm = AlgorithmOption.MFO;
                break;
            default:
                algorithm = AlgorithmOption.Genetic;
                break;
        }
    }
    public static void SwitchCaseArchitecture(string value)
    {
        switch (value)
        {
            case "Tree":
                architecture = ArchitectureOption.Tree;
                break;
            case "NN":
                architecture = ArchitectureOption.NN;
                break;
            default:
                architecture = ArchitectureOption.Tree;
                break;
        }
    }

    public static int SortFunc(Individual a, Individual b)
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

    public static void PrintPopulation(List<Individual> Population, int generation, int mapSize, int index = -1)
    {
        string path = $"{Application.dataPath}/Log/WOA {architecture} {mapSize}x{mapSize}.txt";
        int ctr, maxCtr;
        if (index > 0)
        {
            ctr = index; maxCtr = index + 1;
        }
        else
        {
            ctr = 0; maxCtr = Population.Count;
        }
        string content = "";
        for (int i = ctr; i < maxCtr; i++)
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
