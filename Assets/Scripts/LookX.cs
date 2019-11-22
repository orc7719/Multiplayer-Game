using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookX : MonoBehaviour
{
    [SerializeField]
    float xSensitivity = 5f;

    void Update()
    {
        float mouseX = Input.GetAxisRaw("MouseX");
        mouseX *= xSensitivity;

        transform.localEulerAngles += new Vector3(0, mouseX, 0);
    }
}
