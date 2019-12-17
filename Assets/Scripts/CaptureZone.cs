using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CaptureZone : NetworkBehaviour
{
    List<Player> playersInZone = new List<Player>();

    [SyncVar(hook = "UpdateZone")] int playerCount = 0;

    [SerializeField] GameObject neutralZone;
    [SerializeField] GameObject activeZone;
    [SerializeField] GameObject contestedZone;

    [ServerCallback]
    void Start()
    {
        playersInZone.Clear();

        RpcUpdateZone(0);
            StartCoroutine(DoCaptureTick());
        
    }

    [Server]
    private void FixedUpdate()
    {
        playersInZone.Clear();
    }

    [Server]
    IEnumerator DoCaptureTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            RpcUpdateZone(playersInZone.Count);
            if (playersInZone.Count == 1)
            {
                playersInZone[0].score++;
                if (playersInZone[0].score >= 100)
                    EndGame(playersInZone[0].playerName);
                RpcUpdateScores();
            }
        }
    }

    void EndGame(string playerName)
    {
        for (int i = 0; i < PlayerCanvas.canvas.scoreManager.players.Count; i++)
        {
            PlayerCanvas.canvas.scoreManager.players[i].RpcGameOver(playerName);
        }
    }

    [ClientRpc]
    void RpcUpdateScores()
    {
        PlayerCanvas.canvas.scoreManager.UpdateScores();
    }

    /**
    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();

        if (newPlayer != null)
            if (!playersInZone.Contains(newPlayer))
            playersInZone.Add(newPlayer);

        RpcUpdateZone(playersInZone.Count);
    }
    **/

    [ServerCallback]

    private void OnTriggerStay(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();

        if (newPlayer != null)
            if (!playersInZone.Contains(newPlayer))
                playersInZone.Add(newPlayer);
    }

    /**
    [ServerCallback]
    void OnTriggerExit(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();

        if(newPlayer != null)
        if (playersInZone.Contains(newPlayer))
            playersInZone.Remove(newPlayer);

        RpcUpdateZone(playersInZone.Count);
    }
    **/

    [ClientRpc]
    void RpcUpdateZone(int totalPlayers)
    {
        playerCount = totalPlayers;
    }

    void UpdateZone(int zonePlayers)
    {
        neutralZone.SetActive(zonePlayers == 0);
        activeZone.SetActive(zonePlayers == 1);
        contestedZone.SetActive(zonePlayers >= 2);
    }
}
