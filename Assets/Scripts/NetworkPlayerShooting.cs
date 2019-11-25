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

    [SerializeField] float reloadCooldown = 1f;
    float reloadTimer;

    [SerializeField] NetworkAnimator networkAnim;

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        reloadTimer += Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            if (reloadTimer >= reloadCooldown)
            {
                reloadTimer = 0;
                networkAnim.SetTrigger("Throw");
                CmdShoot();
            }
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
