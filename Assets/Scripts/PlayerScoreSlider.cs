using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScoreSlider : MonoBehaviour
{
    [SerializeField] TMP_Text positionText;
    [SerializeField] Slider scoreSlider;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text scoreText;

    string[] posValues = new string[8] { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th" };

    public void UpdateScoreSlider(int newPos, int score, string newName)
    {
        positionText.text = posValues[newPos];
        scoreSlider.value = score;
        nameText.text = newName;
        scoreText.text = score.ToString();
    }
}