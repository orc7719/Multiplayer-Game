using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField]
    Transform lookTarget;

    void Update()
    {
        transform.LookAt(lookTarget);
    }

    [ContextMenu("Update Rotation")]
    void UpdateRotation()
    {
        transform.LookAt(lookTarget);
    }
}
