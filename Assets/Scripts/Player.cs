using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using TMPro;
using RootMotion.FinalIK;

public class Player : NetworkBehaviour

{
    [SyncVar(hook = "OnNameChanged")] public string playerName;
    [SyncVar(hook = "OnColourChanged")] public Color playerColour;

    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    [SerializeField] float respawnTime = 8f;

    [SerializeField] GameObject respawnParticles;
    [SerializeField] TMP_Text nameText;
    [SyncVar(hook = "OnScoreChanged")] public int score;

    [SerializeField] GameObject ragdollObject;

    GameObject mainCamera;

    public static List<Player> players = new List<Player>();

    bool isAlive;

    void Start()
    {
        ResetPlayer();

        if (isLocalPlayer)
        {
            PlayerCanvas.canvas.scoreManager.AddPlayer(this, true);
            mainCamera = Camera.main.gameObject;
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

    void OnColourChanged(Color newColour)
    {

    }

    [ContextMenu("Kill Player")]
    public void Die()
    {
        Debug.Log("Player: Player " + playerName + " Dead");
        GameObject newRagdoll = Instantiate(ragdollObject, ragdollObject.transform.position, ragdollObject.transform.rotation);
        newRagdoll.GetComponent<PlayerRagdoll>().enabled = true;

        DisablePlayer();

        StartCoroutine(Respawn());

        //Invoke("Respawn", respawnTime);
    }

    IEnumerator Respawn()
    {
        transform.position = Vector3.zero;

        yield return new WaitForSeconds(respawnTime);

        if (isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        yield return null;

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
        }
    }
}

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }
