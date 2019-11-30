using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] disableScripts;
    [SerializeField] MonoBehaviour[] enableScripts;

    [SerializeField] RagdollUtility ragdoll;

    Vector3 hitPoint;

    [SerializeField] Rigidbody[] rigidbodies;

    private void OnEnable()
    {
        for (int i = 0; i < disableScripts.Length; i++)
        {
            Destroy(disableScripts[i]);
        }

        for (int i = 0; i < enableScripts.Length; i++)
        {
            enableScripts[i].enabled = true;
        }
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        StartCoroutine(EnableRagdoll());

        
    }

    public void AddHitPoint(Vector3 damagePoint)
    {
        hitPoint = damagePoint;
    }

    IEnumerator EnableRagdoll()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        ragdoll.EnableRagdoll();

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].AddExplosionForce(200.0f, hitPoint, 5.0f, 3.0f);
        }
    }
}
