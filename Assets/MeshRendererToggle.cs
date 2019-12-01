using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererToggle : MonoBehaviour
{
    [SerializeField] Renderer[] rends;
    private void OnEnable()
    {
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < rends.Length; i++)
        {
            if(rends[i] != null)
            rends[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }
}
