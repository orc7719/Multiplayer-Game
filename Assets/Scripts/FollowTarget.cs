using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform targetPos;

    void Update()
    {
        transform.position = targetPos.position;
    }
}
