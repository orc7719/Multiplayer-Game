using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class LookX : MonoBehaviour
{

    [SerializeField] FloatReference xSensitivity;
    [SerializeField] BoolReference uiLocked;

    void Update()
    {
        if (!uiLocked.Value)
        {
            float mouseX = Input.GetAxisRaw("MouseX");
            mouseX *= xSensitivity.Value;

            transform.localEulerAngles += new Vector3(0, mouseX, 0);
        }
    }
}
