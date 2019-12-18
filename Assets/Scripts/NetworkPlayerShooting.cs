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

    [SerializeField] AudioSource weaponAudio;
    [SerializeField] AudioClip[] shootingSounds;

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
                networkAnim.SetTrigger("Shoot");
                CmdShoot(projectileSpawn.position, projectileSpawn.rotation, aimTarget.position);

                weaponAudio.pitch = Random.Range(0.85f, 0.95f);
                weaponAudio.PlayOneShot(shootingSounds[Random.Range(0, shootingSounds.Length)]);
            }
        }
    }

    [Command]
    void CmdShoot(Vector3 shootPos, Quaternion shootRot, Vector3 shootTarget)
    {
        
        GameObject projectile = Instantiate(projectilePrefab, shootPos, shootRot);
        projectile.transform.LookAt(shootTarget);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileForce;
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
        NetworkServer.Spawn(projectile);
    }
}
