﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker6x6 : MonoBehaviour
{
    private int score;
    public static ScoreTracker6x6 Instance;
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
            if (PlayerPrefs.GetInt("HighScore") < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
                ScoreText.text = score.ToString();
            }
        }
    }
    private void Awake()
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