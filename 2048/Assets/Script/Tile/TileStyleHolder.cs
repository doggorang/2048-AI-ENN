using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileStyle
{
    public int number;
    public Color32 tile_color;
    public Color32 text_color;
}

public class TileStyleHolder : MonoBehaviour
{
    public static TileStyleHolder Instance;
    public TileStyle[] tileStyles;

    private void Awake()
    {
        Instance = this;
    }




}
