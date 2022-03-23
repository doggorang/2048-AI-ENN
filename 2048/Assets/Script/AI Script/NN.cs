using MathNet.Numerics.LinearAlgebra;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN
{
    // 6 input layer sesuai dengan ukuran juga
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 6); // ini 6 input layer
    //public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 16); // ini coba input layer map size 4x4
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
                weights.Add(Matrix<float>.Build.Dense(6, hiddenNeuronCount)); // ini 6 input layer
                //weights.Add(Matrix<float>.Build.Dense(16, hiddenNeuronCount)); // ini coba input layer map size 4x4
            }
            else
            {
                weights.Add(Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount));
            }
        }
        // terkahir weight untuk outptut layer
        weights.Add(Matrix<float>.Build.Dense(hiddenNeuronCount, 4));
        biases.Add(w[ctrIdxWeight++]);

        // Isi weight sesuai dari parameter
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

    public MoveDirection[] Move(float il0, float il1, float il2, float il3, float il4, float il5)
    {
        // masukin input layer
        //for (int i = 0; i < 16; i++)// ini coba input layer map size 4x4 float[] IL
        //{
        //    inputLayer[0, i] = IL[i];
        //}
        inputLayer[0, 0] = il0; // ini 6 input layer
        inputLayer[0, 1] = il1;
        inputLayer[0, 2] = il2;
        inputLayer[0, 3] = il3;
        inputLayer[0, 4] = il4;
        inputLayer[0, 5] = il5;
        // PointwiseTanh supaya hasil angkanya nanti range -1 sampai 1 sedangkan digmoid supaya hasil angkanya 0 sampai 1
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
        MoveDirection[] ret = new MoveDirection[4] { MoveDirection.Left, MoveDirection.Up, MoveDirection.Right, MoveDirection.Down };
        float[] score = new float[4] { Sigmoid(outputLayer[0, 0]), Sigmoid(outputLayer[0, 1]), Sigmoid(outputLayer[0, 2]), Sigmoid(outputLayer[0, 3]) };
        // sorting sesuai order score dan move direction
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3 - i; j++)
            {
                if (score[j] < score[j + 1])
                {
                    float temp = score[j];
                    MoveDirection tempMd = ret[j];

                    score[j] = score[j + 1];
                    ret[j] = ret[j + 1];

                    score[j + 1] = temp;
                    ret[j + 1] = tempMd;
                }
            }
        }
        return ret;
    }

    private float Sigmoid(float value)
    {
        return (1 / (1 + Mathf.Exp(-value)));
    }
}
