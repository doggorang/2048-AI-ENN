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
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        ScoreText.text = "0";
        HighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
