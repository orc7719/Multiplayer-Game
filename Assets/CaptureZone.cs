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
    IEnumerator DoCaptureTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (playersInZone.Count == 1)
                playersInZone[0].score++;

            Debug.Log(playersInZone.Count);
        }
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();

        if (newPlayer != null)
            if (!playersInZone.Contains(newPlayer))
            playersInZone.Add(newPlayer);

        RpcUpdateZone(playersInZone.Count);
    }

    [ServerCallback]
    void OnTriggerExit(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();

        if(newPlayer != null)
        if (playersInZone.Contains(newPlayer))
            playersInZone.Remove(newPlayer);

        RpcUpdateZone(playersInZone.Count);
    }

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
