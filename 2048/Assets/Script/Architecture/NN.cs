using MathNet.Numerics.LinearAlgebra;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN : MonoBehaviour
{
    // 6 input layer sesuai dengan ukuran juga
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 6);
    // hidden layer jadi list karena ukuran bisa berubah"
    public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();
    // 4 output layer sesuai dengan output game
    public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 4);
    // weight setiap hubungan layer
    public List<Matrix<float>> weights = new List<Matrix<float>>();
    // bias hidden layer
    public List<float> biases = new List<float>();

    public void Initialize(int hiddenLayerCount, int hiddenNeuronCount)
    {
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();
        for (int i = 0; i < hiddenLayerCount; i++)
        {
            hiddenLayers.Add(Matrix<float>.Build.Dense(1, hiddenNeuronCount));
            biases.Add(Random.Range(-1f, 1f));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
