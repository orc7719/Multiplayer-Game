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

    [SerializeField] GameObject joinMenu;

    public TMP_InputField nameInput;

    public TMP_Text winnerText;
    public GameObject winnerHolder;

    private void Start()
    {
        if (canvas == null)
            canvas = this;
    }

    private void OnEnable()
    {
        nameInput.onEndEdit.AddListener(UpdatePlayerName);
    }

    private void OnDisable()
    {
        nameInput.onEndEdit.RemoveListener(UpdatePlayerName);
    }

    public void UpdatePlayerScore(int newScore)
    {
        playerScoreSlider.value = newScore;
        playerScoreText.text = newScore.ToString();
    }

    public void UpdatePlayerName(string newName)
    {
        scoreManager.localPlayer.ChangeName(newName);
    }

    public void StartGame()
    {
        joinMenu.SetActive(false);
        scoreManager.localPlayer.CmdSpawnPlayer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowWinner(string winnerName)
    {
        winnerText.text = winnerName + " Wins!";
        winnerHolder.SetActive(true);
    }
}
