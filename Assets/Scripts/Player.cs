using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Player : NetworkBehaviour

{
    [SyncVar(hook = "OnNameChanged")] public string playerName;

    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    [SerializeField] float respawnTime = 8f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResetPlayer();

        EnablePlayer();
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

    }

    public void Die()
    {
        Debug.Log("Player: Player " + playerName + " Dead");
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
