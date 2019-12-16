using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] disableScripts;
    [SerializeField] MonoBehaviour[] enableScripts;

    [SerializeField] RagdollUtility ragdoll;

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
        StartCoroutine(EnableRagdoll());

        
    }

    IEnumerator EnableRagdoll()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        ragdoll.EnableRagdoll();

        yield return new WaitForSeconds(5f);

        Rigidbody[] rgdbodies = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rgdbodies.Length; i++)
        {
            rgdbodies[i].useGravity = false;
        }

        int randomBody = Random.Range(0, rgdbodies.Length);

        ConstantForce bodyForce = rgdbodies[randomBody].gameObject.AddComponent<ConstantForce>();
        bodyForce.force = new Vector3(0, 5, 0);

        Destroy(gameObject, 30f);
    }
}
