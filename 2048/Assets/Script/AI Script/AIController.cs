using System.Collections;
using System.Collections.Generic;
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
}
