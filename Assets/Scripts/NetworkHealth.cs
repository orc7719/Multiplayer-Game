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

    [ServerCallback]
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void Damage(int value, Vector3 damagePoint)
    {
        bool isDead = false;

        currentHealth -= value;
        isDead = currentHealth <= 0;

        RpcTakeDamage(isDead, damagePoint);
    }

    [ClientRpc]
    void RpcTakeDamage(bool dead, Vector3 damagePoint)
    {
        if (dead)
            player.Die(damagePoint);
    }

    void OnHealthChanged(int newHealth)
    {
        currentHealth = newHealth;
    }
}
