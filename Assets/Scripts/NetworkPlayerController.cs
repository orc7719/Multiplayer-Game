using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 4;
    [SerializeField] float sprintSpeed = 6;
    [SerializeField] float jumpHeight = 5;
    [SerializeField] float gravity = 12;

    CharacterController charController;
    [SerializeField] Animator anim;
    [SerializeField] Animator firstAnim;

    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip[] jumpSounds;

    [SerializeField] AudioSource footstepAudio;
    [SerializeField] AudioClip[] footstepSounds;
    [SerializeField] float footstepCooldown;
    float footstepTimer;

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



        if (charController.isGrounded)
        {
            if (footstepTimer >= footstepCooldown)
            {
                footstepAudio.pitch = Random.Range(0.9f, 1.1f);
                footstepAudio.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
                footstepTimer = 0f;
            }

            footstepTimer += charVelocity.magnitude * Time.deltaTime;
        }
        else
            footstepTimer = footstepCooldown;

        if(isLocalPlayer)
            DoFirstPersonAnimation();
    }

    void DoFirstPersonAnimation()
    {
        firstAnim.SetFloat("MoveX", playerVelocity.x);
        firstAnim.SetFloat("MoveZ", playerVelocity.z);
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
        move *= Input.GetButton("Sprint") ? sprintSpeed : moveSpeed;

        if (h != 0 && v != 0)
            move *= 0.7f;

        if(charController.isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpHeight;
                playerAudio.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
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
