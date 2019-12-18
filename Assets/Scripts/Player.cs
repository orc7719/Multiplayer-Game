using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using TMPro;
using RootMotion.FinalIK;

public class Player : NetworkBehaviour

{
    [Header("Player Info")]
    [SyncVar(hook = "OnNameChanged")] public string playerName;
    [SyncVar(hook = "OnColourChanged")] public int colourID;
    [SyncVar(hook = "OnMaskChanged")] public int maskID;

    [Header("Toggle Events")]
    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    [SerializeField] float respawnTime = 8f;

    [Header("Player Components")]
    [SerializeField] GameObject respawnParticles;
    [SerializeField] TMP_Text nameText;
    [SyncVar(hook = "OnScoreChanged")] public int score;

    [SerializeField] GameObject ragdollObject;

    [Header("Player Visuals")]
    [SerializeField] Material[] playerMats;
    [SerializeField] Renderer[] playerRenderers;

    [Header("Player Audio")]
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip[] deathSounds;

    GameObject mainCamera;

    bool playerSetup = false;

    public static List<Player> players = new List<Player>();

    bool isAlive;

    void Start()
    {
        ResetPlayer();
        EnablePlayer();

        if (isLocalPlayer)
        {

        }
    }

    private void OnEnable()
    {
        PlayerCanvas.canvas.scoreManager.AddPlayer(this, isLocalPlayer);
        
    }

    private void OnDisable()
    {
        PlayerCanvas.canvas.scoreManager.RemovePlayer(this, isLocalPlayer);
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if(!playerSetup)
        {
            Debug.Log("Set Local Player");
            PlayerCanvas.canvas.scoreManager.AddPlayer(this, true);
            mainCamera = Camera.main.gameObject;
            DisablePlayer();
            playerSetup = true;
        }

        if (isAlive)
        {
            if (Input.GetMouseButton(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    
    }

    void ResetPlayer()
    {
        onToggleShared.Invoke(false);
            onToggleLocal.Invoke(false);
            onToggleRemote.Invoke(false);
    }

    [Command]
    public void CmdSpawnPlayer()
    {
        RpcSpawnPlayer();
    }

    [ClientRpc]
    public void RpcSpawnPlayer()
    {
        EnablePlayer();
    }

    void EnablePlayer()
    {
        isAlive = true;

        onToggleShared.Invoke(true);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(true);
            mainCamera.SetActive(false);
        }
        else
            onToggleRemote.Invoke(true);
    }

    void DisablePlayer()
    {
        isAlive = false;
        onToggleShared.Invoke(false);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(false);
            mainCamera.SetActive(true);
        }
        else
            onToggleRemote.Invoke(false);
    }

    public void ChangeName(string newName)
    {
        string localName = newName; //Used for checking name before sending to server

        CmdChangeName(localName);
    }

    [Command]
    void CmdChangeName(string newName)
    {
        playerName = newName;
    }

    void OnNameChanged(string newName)
    {
        nameText.text = newName;
    }

    void OnScoreChanged(int newScore)
    {
        if (isLocalPlayer)
            PlayerCanvas.canvas.UpdatePlayerScore(newScore);
    }

    void OnColourChanged(int newColour)
    {
        for (int i = 0; i < playerRenderers.Length; i++)
        {
            playerRenderers[i].material = playerMats[newColour];
        }
    }

    void OnMaskChanged(int newMask)
    {

    }

    [ContextMenu("Kill Player")]
    public void Die()
    {
        Debug.Log("Player: Player " + playerName + " Dead");
        GameObject newRagdoll = Instantiate(ragdollObject, ragdollObject.transform.position, ragdollObject.transform.rotation);
        newRagdoll.GetComponent<PlayerRagdoll>().enabled = true;

        if (isLocalPlayer)
            playerAudio.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);

        DisablePlayer();

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        if (isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        yield return new WaitForSeconds(respawnTime);

        EnablePlayer();  
    }

    [ClientRpc]
    public void RpcGameOver(string winningPlayer)
    {
        DisablePlayer();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (isLocalPlayer)
        {
            PlayerCanvas.canvas.ShowWinner(winningPlayer);
            StartCoroutine(PlayerDisconnect());
        }
    }

    IEnumerator PlayerDisconnect()
    {
        yield return new WaitForSeconds(5f);

        NetworkManager.singleton.StopClient();
    }
}

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }
