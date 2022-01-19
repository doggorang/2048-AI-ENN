using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public void PlayGameStage1()
    {
        string algo = "Genetic", archi = "Tree";
        // get radio button checked untuk algorithm
        GameObject GOAlgo = GameObject.Find("AlgorithmOption");
        for (int i = 0; i < GOAlgo.transform.childCount; i++)
        {
            if (GOAlgo.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                algo = GOAlgo.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
            }
        }
        // get radio button checked untuk architecture
        GameObject GOArchi = GameObject.Find("ArchitectureOption");
        for (int i = 0; i < GOArchi.transform.childCount; i++)
        {
            if (GOArchi.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                archi = GOArchi.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
            }
        }
        AIController.algorithm = algo;
        AIController.architecture = archi;
        Debug.Log(AIController.algorithm);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayGameStage2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayGameStage3()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
