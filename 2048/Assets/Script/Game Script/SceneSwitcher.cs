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
        AIController.SwitchCaseAlgorithm(algo);
        AIController.SwitchCaseArchitecture(archi);
        SceneManager.LoadScene(1);
    }
    public void PlayGameStage2()
    {
        SceneManager.LoadScene(2);
    }
    public void PlayGameStage3()
    {
        SceneManager.LoadScene(3);
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
}
