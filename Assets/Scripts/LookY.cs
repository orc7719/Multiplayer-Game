using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class LookY : MonoBehaviour
{
    [SerializeField] FloatReference ySensitivity;
    [SerializeField] BoolReference uiLocked;

    [SerializeField] float maxX = 80f;
    [SerializeField] float minX = -80f;

    void Update()
    {
        if (!uiLocked.Value)
        {
            float mouseY = Input.GetAxisRaw("MouseY");
            mouseY *= ySensitivity.Value;

            float xAngle = transform.eulerAngles.x - mouseY;

            if (xAngle > 180)
            {
                xAngle -= 360;
            }

            xAngle = Mathf.Clamp(xAngle, minX, maxX);

            transform.eulerAngles = new Vector3(xAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
