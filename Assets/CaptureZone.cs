using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CaptureZone : NetworkBehaviour
{
    [SyncVar(hook = "UpdateZone")]List<Player> playersInZone = new List<Player>();

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

        UpdateZone(playersInZone);

        //RpcUpdateZone(playersInZone.Count);
    }

    [ServerCallback]
    void OnTriggerExit(Collider other)
    {
        Player newPlayer = other.GetComponent<Player>();

        if(newPlayer != null)
        if (playersInZone.Contains(newPlayer))
            playersInZone.Remove(newPlayer);

        UpdateZone(playersInZone);

        //RpcUpdateZone(playersInZone.Count);
    }

    [ClientRpc]
    void RpcUpdateZone(int zoneId)
    {
        neutralZone.SetActive(zoneId == 0);
        activeZone.SetActive(zoneId == 1);
        contestedZone.SetActive(zoneId >= 2);
    }

    void UpdateZone(List<Player> playerList)
    {
        neutralZone.SetActive(playerList.Count == 0);
        activeZone.SetActive(playerList.Count == 1);
        contestedZone.SetActive(playerList.Count >= 2);
    }
}
