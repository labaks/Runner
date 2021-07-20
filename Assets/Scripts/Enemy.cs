using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth, speed, knockBackDuration;
    [SerializeField] private float groundCheckDistance, wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck, wallCheck;
    [SerializeField] private Vector3 knockBackSpeed;
    private bool groundDetected, wallDetected;
    private float currentHealth, knockBackStartTime;
    private int facingDirection, damageDirection;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;

    private enum State
    {
        Walking, Knockback, Dead
    }

    private State currentState;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        facingDirection = 1;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                UpdateWalkingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    private void EnterWalkingState()
    {

    }
    private void UpdateWalkingState()
    {
        groundDetected = Physics.CheckSphere(groundCheck.position, groundCheckDistance, whatIsGround, QueryTriggerInteraction.Ignore);
        wallDetected = Physics.CheckSphere(wallCheck.position, wallCheckDistance, whatIsGround, QueryTriggerInteraction.Ignore);

        if (!groundDetected || wallDetected)
        {
            Flip();
        }
        else
        {
            movement.Set(-speed * facingDirection, rb.velocity.y, rb.velocity.z);
            rb.velocity = movement;
        }
    }
    private void ExitWalkingState()
    {

    }

    private void EnterKnockbackState()
    {
        // Debug.Log("EnterKnockbackState");
        knockBackStartTime = Time.time;
        movement.Set(knockBackSpeed.x * damageDirection, knockBackSpeed.y, knockBackSpeed.z);
        rb.velocity = movement;
        animator.SetBool("knockback", true);
    }
    private void UpdateKnockbackState()
    {
        if (Time.time >= knockBackStartTime + knockBackDuration)
        {
            SwitchState(State.Walking);
        }

    }
    private void ExitKnockbackState()
    {
        animator.SetBool("knockback", false);
    }

    private void EnterDeadState()
    {

    }
    private void UpdateDeadState()
    {

    }
    private void ExitDeadState()
    {

    }

    void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                ExitWalkingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Walking:
                EnterWalkingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
        currentState = state;
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];

        if (attackDetails[1] > transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        //Hit particle

        if (currentHealth > 0.0f)
        {
            SwitchState(State.Knockback);
        }
        else
        {
            SwitchState(State.Dead);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x, wallCheck.position.y - wallCheckDistance));
    }
}
