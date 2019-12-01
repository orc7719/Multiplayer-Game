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
    }
}
