using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    NetworkManager networkManager;

    [SerializeField] TMP_InputField ipInput;

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void StartServer()
    {
        networkManager.StartHost();
    }

    public void JoinServer()
    {
        if (ipInput.text.Length > 0)
        {
            networkManager.networkAddress = ipInput.text;
            networkManager.StartClient();
        }
    }
}
