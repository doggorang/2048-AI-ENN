using MathNet.Numerics.LinearAlgebra;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN
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

    public NN(List<float> w, int hiddenLayerCount, int hiddenNeuronCount)
    {
        int ctrIdxWeight = 0;
        for (int i = 0; i < hiddenLayerCount; i++)
        {
            // buat hidden layer
            hiddenLayers.Add(Matrix<float>.Build.Dense(1, hiddenNeuronCount));
            //biases.Add(Random.Range(-1f, 1f));
            biases.Add(w[ctrIdxWeight++]);
            // i = 0 berarti weight hubungan pada input layer
            if (i == 0)
            {
                weights.Add(Matrix<float>.Build.Dense(6, hiddenNeuronCount));
            }
            else
            {
                weights.Add(Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount));
            }
        }
        // terkahir weight untuk outptut layer
        weights.Add(Matrix<float>.Build.Dense(hiddenNeuronCount, 4));
        biases.Add(w[ctrIdxWeight++]);

        // Random isi weight
        foreach (Matrix<float> wei in weights)
        {
            for (int i = 0; i < wei.RowCount; i++)
            {
                for (int j = 0; j < wei.ColumnCount; j++)
                {
                    wei[i, j] = w[ctrIdxWeight++];
                }
            }
        }
    }

    public MoveDirection Move(float il0, float il1, float il2, float il3, float il4, float il5)
    {
        // masukin input layer
        inputLayer[0, 0] = il0;
        inputLayer[0, 1] = il1;
        inputLayer[0, 2] = il2;
        inputLayer[0, 3] = il3;
        inputLayer[0, 4] = il4;
        inputLayer[0, 5] = il5;
        inputLayer = inputLayer.PointwiseTanh();
        // hitung hidden layer
        hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();
        for (int i = 1; i < hiddenLayers.Count; i++)
        {
            hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }
        // hitung output layer
        outputLayer = ((hiddenLayers[hiddenLayers.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh();
        // biggest score return direction
        MoveDirection ret = MoveDirection.Left;
        float score = float.MinValue;
        if (Sigmoid(outputLayer[0, 0]) > score)
        {
            score = outputLayer[0, 0];
            ret = MoveDirection.Left;
        }
        if (Sigmoid(outputLayer[0, 1]) > score)
        {
            score = outputLayer[0, 1];
            ret = MoveDirection.Up;
        }
        if (Sigmoid(outputLayer[0, 2]) > score)
        {
            score = outputLayer[0, 2];
            ret = MoveDirection.Right;
        }
        if (Sigmoid(outputLayer[0, 3]) > score)
        {
            score = outputLayer[0, 3];
            ret = MoveDirection.Down;
        }
        return ret;
    }

    private float Sigmoid(float value)
    {
        return (1 / (1 + Mathf.Exp(-value)));
    }
}
