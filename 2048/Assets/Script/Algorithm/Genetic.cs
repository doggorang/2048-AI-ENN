using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genetic : MonoBehaviour
{
    private List<Individual> AllIndivual = new List<Individual>();
    private int population = 10;
    private Individual fittest;
    private Individual secondFittest;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(218116692);
        for (int i = 0; i < population; i++)
        {
            float[] tempW = new float[6];
            for (int j = 0; j < 6; j++)
            {
                tempW[j] = Random.value;
            }
            AllIndivual.Add(new Individual(tempW));
        }
    }
    public void Selection()
    {
        fittest = AllIndivual[0];
        secondFittest = AllIndivual[1];
    }
    public void Crossover()
    {

    }
    public void Mutation()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
