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
    [SerializeField] Animator anim;

    float yVelocity;
    public Vector3 playerVelocity;
    Vector3 previousPos;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        previousPos = transform.position;
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
        Vector3 charVelocity = transform.InverseTransformDirection((transform.position - previousPos) / Time.deltaTime);
        previousPos = transform.position;
        playerVelocity = Vector3.Lerp(playerVelocity, charVelocity, 10 * Time.deltaTime);

        anim.SetFloat("MoveX", playerVelocity.x);
        anim.SetFloat("MoveZ", playerVelocity.z);
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

        //if (h != 0 && v != 0)
        //    move *= 0.7f;

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
