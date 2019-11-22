using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    [SerializeField] float ySensitivity = 5f;

    [SerializeField] float maxX = 80f;
    [SerializeField] float minX= -80f;

    void Update()
    {
        float mouseY = Input.GetAxisRaw("MouseY");
        mouseY *= ySensitivity;

        float xAngle = transform.eulerAngles.x - mouseY;

        if(xAngle > 180)
        {
            xAngle -= 360;
        }

        xAngle = Mathf.Clamp(xAngle, minX, maxX);

        transform.eulerAngles = new Vector3(xAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
