using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas canvas;

    public ScoreManager scoreManager;

    public Slider playerScoreSlider;
    public TMP_Text playerScoreText;

    private void Start()
    {
        if (canvas == null)
            canvas = this;
    }

    public void UpdatePlayerScore(int newScore)
    {
        playerScoreSlider.value = newScore;
        playerScoreText.text = newScore.ToString();
    }
}
