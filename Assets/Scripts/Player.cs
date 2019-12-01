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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResetPlayer();

        EnablePlayer();

        if (isLocalPlayer)
            PlayerCanvas.canvas.scoreManager.AddPlayer(this, true);
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

        if(Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void ResetPlayer()
    {
        onToggleShared.Invoke(false);
            onToggleLocal.Invoke(false);
            onToggleRemote.Invoke(false);
    }

    void EnablePlayer()
    {
        onToggleShared.Invoke(true);

        if (isLocalPlayer)
            onToggleLocal.Invoke(true);
        else
            onToggleRemote.Invoke(true);
    }

    void DisablePlayer()
    {
        onToggleShared.Invoke(false);

        if (isLocalPlayer)
            onToggleLocal.Invoke(false);
        else
            onToggleRemote.Invoke(false);
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

        Invoke("Respawn", respawnTime);


    }

    void Respawn()
    {
        EnablePlayer();  

        if(isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }
    }
}

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }
