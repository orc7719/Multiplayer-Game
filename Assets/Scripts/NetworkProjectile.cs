using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkProjectile : MonoBehaviour
{
    [SerializeField] GameObject particlePrefab;

    private void Start()
    {
        transform.rotation = Random.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.tag == "Player")
        {
            NetworkHealth health = collision.transform.root.GetComponent<NetworkHealth>();
            if(health != null)
            {
                health.Damage(100);
            }
        }

        Quaternion hitRotation = Quaternion.FromToRotation(Vector3.forward, collision.contacts[0].normal);
        Instantiate(particlePrefab, collision.contacts[0].point + (collision.contacts[0].normal * 0.05f), hitRotation);
        
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        
        Destroy(gameObject, 1);
    }
}
