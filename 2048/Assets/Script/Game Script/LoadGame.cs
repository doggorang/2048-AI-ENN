using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartDLL;

public class LoadGame : MonoBehaviour
{
    public Text TextFileName;
    private SmartFileExplorer fileExplorer = new SmartFileExplorer();
    private bool readText = false;
    private void OpenFileExplorer()
    {
        fileExplorer.OpenExplorer(Application.dataPath, true, "Open Individual", "json", "json files (*.json)|*.json");
        readText = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fileExplorer.resultOK && readText)
        {
            string path = fileExplorer.fileName;
            AIController.path = path;
            int ctrSubStr = path.LastIndexOf("2048");
            TextFileName.text = path.Substring(ctrSubStr, path.Length - ctrSubStr);
            readText = false;
        }
    }
}
