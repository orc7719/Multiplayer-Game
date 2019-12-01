using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<Player> players;

    public Player localPlayer;
    public Animator scoresAnim;

    [SerializeField] PlayerScoreSlider playerScore;
    [SerializeField] PlayerScoreSlider otherScore;

    private void Start()
    {
        UpdateScores();
    }

    public void AddPlayer(Player newPlayer, bool isLocal)
    {
        Debug.Log("Adding Player, is local? : " + isLocal);
        if (!players.Contains(newPlayer))
            players.Add(newPlayer);

        if (isLocal)
        {
            localPlayer = newPlayer;
            Debug.Log("Local Player Added");
        }

        UpdateScores();
    }
    public void RemovePlayer(Player newPlayer, bool isLocal)
    {
        if (players.Contains(newPlayer))
            players.Remove(newPlayer);

        if (isLocal)
        {
            localPlayer = null;

            Debug.Log("Local Player Removed");
        }

        UpdateScores();
    }

    public void UpdateScores()
    {
        if (players.Count > 1)
        {
            otherScore.gameObject.SetActive(true);
            players.Sort(SortByScore);

            Player otherPlayer = players[0].isLocalPlayer ? players[1] : players[0];

            playerScore.UpdateScoreSlider(players.IndexOf(localPlayer), localPlayer.score, "You");
            otherScore.UpdateScoreSlider(players.IndexOf(otherPlayer), otherPlayer.score, otherPlayer.playerName);

            scoresAnim.SetBool("PlayerLead", localPlayer.score > otherPlayer.score);
        }
        else if(localPlayer != null)
        {
            playerScore.UpdateScoreSlider(players.IndexOf(localPlayer), localPlayer.score, "You");
            otherScore.gameObject.SetActive(false);
        }
    }

    static int SortByScore(Player player1, Player player2)
    {
        return player2.score.CompareTo(player1.score);
    }
}
