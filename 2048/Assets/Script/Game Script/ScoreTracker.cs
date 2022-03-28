using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    private int score;
    public static ScoreTracker Instance;
    public int mapSize;
    public Text ScoreText;
    public Text HighScoreText;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            ScoreText.text = score.ToString();
            if (PlayerPrefs.GetInt($"HighScore{mapSize}") < score)
            {
                PlayerPrefs.SetInt($"HighScore{mapSize}", score);
                HighScoreText.text = score.ToString();
            }
        }
    }
    private void Awake()
    {
        Instance = this;
        if (!PlayerPrefs.HasKey("HighScore4"))
        {
            PlayerPrefs.SetInt("HighScore4", score);
        }
        if (!PlayerPrefs.HasKey("HighScore5"))
        {
            PlayerPrefs.SetInt("HighScore5", score);
        }
        if (!PlayerPrefs.HasKey("HighScore6"))
        {
            PlayerPrefs.SetInt("HighScore6", score);
        }
        ScoreText.text = "0";
        HighScoreText.text = PlayerPrefs.GetInt($"HighScore{mapSize}").ToString();
    }
}
