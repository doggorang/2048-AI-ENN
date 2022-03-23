using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Playing,
    GameOver,
    WaitingForMoveToEnd
}

public enum MoveDirection
{
    // urutan move direction Left, Up, Right, Down
    Left, Up, Right, Down
}

public class SceneSwitcher : MonoBehaviour
{
    private int SceneDestination = 1;
    public GameObject NNPanel;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneDestination);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadGameSceneSwitcher()
    {
        SceneManager.LoadScene(4);
    }
    public void LoadGame()
    {
        string mapSize = "Genetic";
        int sceneNum;
        GameObject GOMapSize = GameObject.Find("MapSizeOption");
        for (int i = 0; i < GOMapSize.transform.childCount; i++)
        {
            if (GOMapSize.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                mapSize = GOMapSize.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
            }
        }
        switch (mapSize)
        {
            case "4x4":
                sceneNum = 1;
                break;
            case "5x5":
                sceneNum = 2;
                break;
            case "6x6":
                sceneNum = 3;
                break;
            default:
                sceneNum = 1;
                break;
        }
        SceneManager.LoadScene(sceneNum);
    }
    public void HandleDropdownAlgorithm(int val)
    {
        if (val == 0)
            AIController.algorithm = AlgorithmOption.Genetic;
        else if (val == 1)
            AIController.algorithm = AlgorithmOption.WOA;
        else if (val == 2)
            AIController.algorithm = AlgorithmOption.MFO;
    }
    public void HandleDropdownArchitecture(int val)
    {
        if (val == 0)
        {
            AIController.architecture = ArchitectureOption.Tree;
            NNPanel.SetActive(false);
        }
        else if (val == 1)
        {
            AIController.architecture = ArchitectureOption.NN;
            NNPanel.SetActive(true);
        }
    }
    public void HandleDropdownMap(int val)
    {
        if (val == 0)
            SceneDestination = 1;
        else if (val == 1)
            SceneDestination = 2;
        else if (val == 2)
            SceneDestination = 3;
    }
    public void ReadIFGen(string val)
    {
        AIController.ngens = int.Parse(val);
    }
    public void ReadIFPop(string val)
    {
        AIController.populationSize = int.Parse(val);
    }
    public void ReadIFLayer(string val)
    {
        AIController.layer = int.Parse(val);
    }
    public void ReadIFNeuron(string val)
    {
        AIController.neuron = int.Parse(val);
    }
}
