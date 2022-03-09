using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public Text TextFileName;
    public void OpenFileExplorer()
    {
        string path = EditorUtility.OpenFilePanel("Open Individual", Application.dataPath, "json");
        AIController.path = path;
        int ctrSubStr = path.LastIndexOf("2048");
        TextFileName.text = path.Substring(ctrSubStr, path.Length - ctrSubStr);
    }
}
