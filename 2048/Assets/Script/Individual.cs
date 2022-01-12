using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour
{
    public float Fitness;
    private float[] weights = new float[6];
    public Individual(float[] weights)
    {
        this.weights = weights;
    }
    public float[] Weights
    {
        get
        {
            return weights;
        }
        set
        {
            weights = value;
            Fitness = 1;
        }
    }

    private void Awake()
    {

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
