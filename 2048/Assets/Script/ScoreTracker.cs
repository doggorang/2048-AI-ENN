using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    private int score;
    public static ScoreTracker Instance;
    public Text ScoreText;
    public Text HighScoreText;
    void Awake()
    {
        Instance = this;
    }
}
