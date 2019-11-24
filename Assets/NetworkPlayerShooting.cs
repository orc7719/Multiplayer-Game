using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerShooting : NetworkBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] float projectileForce;

    [SerializeField] Animator anim;

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if(Input.GetButtonDown("Fire1"))
        {
            CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {
        anim.SetTrigger("Throw");

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileForce;
        NetworkServer.Spawn(projectile);
    }
}
