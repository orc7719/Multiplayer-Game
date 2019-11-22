using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpHeight = 5;
    [SerializeField] float gravity = 12;

    CharacterController charController;

    float yVelocity;

    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        DoAnimation();

        if (!isLocalPlayer)
            return;

        DoMovement();
        DoInteraction();
    }

    void DoAnimation()
    {

    }

    void DoInteraction()
    {

    }

    void DoMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        move = transform.TransformDirection(move);
        move *= moveSpeed;

        if (h != 0 && v != 0)
            move *= 0.7f;

        if(charController.isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpHeight;
            }
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
        }

        move.y = yVelocity;
        charController.Move(move * Time.deltaTime);
    }
}
