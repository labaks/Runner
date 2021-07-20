using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private float inputTimer, attackRadius, attackDamage;
    [SerializeField] private Transform attackHitboxPos;
    [SerializeField] private LayerMask whatIsDamagable;

    private bool gotInput, isAttacking;
    private float lastInputTime = Mathf.NegativeInfinity;

    private float[] attackDetails = new float[2];
    private bool isAttackPressed = false;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (isAttackPressed)
        {
            gotInput = true;
            lastInputTime = Time.time;
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                isAttackPressed = false;
                gotInput = false;
                isAttacking = true;
                anim.SetBool("isAttack", isAttacking);
                CheckAttackHitBox();
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
            FinishAttack();
        }
    }

    private void CheckAttackHitBox()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(attackHitboxPos.position, attackRadius, whatIsDamagable, QueryTriggerInteraction.Ignore);

        attackDetails[0] = attackDamage;
        attackDetails[1] = transform.position.x;
        foreach (Collider colider in detectedObjects)
        {
            Debug.Log(colider);
            colider.SendMessage("Damage", attackDetails);
            //hitParticles
        }
    }

    public void Attack()
    {
        isAttackPressed = true;
    }

    private void FinishAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttack", isAttacking);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackHitboxPos.position, attackRadius);
    }
}
