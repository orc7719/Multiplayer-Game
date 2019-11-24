using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerShooting : NetworkBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] Transform aimTarget;
    [SerializeField] float projectileForce;

    [SerializeField] NetworkAnimator networkAnim;
    NetworkPlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<NetworkPlayerController>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if(Input.GetButtonDown("Fire1"))
        {
            networkAnim.SetTrigger("Throw");
            CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        projectile.transform.LookAt(aimTarget);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileForce;
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
        NetworkServer.Spawn(projectile);
    }
}
