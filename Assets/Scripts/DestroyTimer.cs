using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] float destroyTime = 5;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
