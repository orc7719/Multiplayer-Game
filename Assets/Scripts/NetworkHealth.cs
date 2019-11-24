using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkHealth : NetworkBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SyncVar(hook = "OnHealthChanged")] int currentHealth;

    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    [Server]
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void Damage(int value)
    {
        bool isDead = false;

        currentHealth -= value;
        isDead = currentHealth <= 0;

        RpcTakeDamage(isDead);
    }

    [ClientRpc]
    void RpcTakeDamage(bool dead)
    {
        if (dead)
            player.Die();
    }

    void OnHealthChanged(int newHealth)
    {
        currentHealth = newHealth;
    }
}
