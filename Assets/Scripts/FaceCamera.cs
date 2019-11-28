using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.current.transform);
        Vector3 objectRot = transform.localEulerAngles;
        objectRot.x = 0;
        transform.localEulerAngles = objectRot;
    }
}
