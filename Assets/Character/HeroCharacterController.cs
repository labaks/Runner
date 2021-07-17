using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacterController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    [SerializeField] public float actualSpeed = 8f;
    [SerializeField] public float speed = 8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private Transform[] groundChecks;
    [SerializeField] private Transform[] wallChecks;
    [SerializeField] private GameObject gameManager;
    public float heroHealth = 100f;
    private float gravity = -50f;
    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;
    private bool blocked;
    public float horizontalInput;

    private bool jumpPressed;
    private float jumpTimer;
    private float jumpGracePeriod = 0.2f;

    private GameManager GM;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        GM = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = new Vector3(horizontalInput, 0, 90);

        isGrounded = false;

        foreach (var groundCheck in groundChecks)
        {
            if (Physics.CheckSphere(groundCheck.position, 0.01f, groundLayers, QueryTriggerInteraction.Ignore))
            {
                isGrounded = true;
                break;
            }
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        blocked = false;
        foreach (var wallCheck in wallChecks)
        {
            if (Physics.CheckSphere(wallCheck.position, 0.01f, groundLayers, QueryTriggerInteraction.Ignore))
            {
                blocked = true;
                break;
            }
        }

        // movement
        if (!blocked)
        {
            characterController.Move(new Vector3(horizontalInput * actualSpeed, 0, 0) * Time.deltaTime);
        }

        // jumpPressed = Input.GetButtonDown("Jump");

        if (jumpPressed)
        {
            jumpTimer = Time.time;
        }

        if (isGrounded && (jumpPressed || (jumpTimer > 0 && Time.time < jumpTimer + jumpGracePeriod)))
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            jumpTimer = -1;
            jumpPressed = false;
        }
        // add gravitation
        characterController.Move(velocity * Time.deltaTime);

        animator.SetFloat("speed", actualSpeed);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("verticalSpeed", velocity.y);

        if (heroHealth <= 0)
        {
            heroDeath();
        }

    }
    private void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "DeathZone":
                heroHealth = 0;
                GM.finishGame();
                break;
            case "Finish":
                actualSpeed = 0;
                GM.finishGame();
                break;
            default: break;
        }
    }

    public void heroDeath()
    {
        gameObject.SetActive(false);
    }

    public void JumpPressed()
    {
        jumpPressed = true;
    }
}
