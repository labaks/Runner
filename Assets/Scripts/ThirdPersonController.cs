using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    public float speed = 6f;
    public int maxHealth = 100;
    [SerializeField] int currentHealth;
    public Image healthImg;
    public Text healthText;
    public float turnSmoothTime = 0.1f;
    public int attackDamage = 40;
    float turnSmoothVelocity;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        else
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            animator.SetFloat("speed", direction.magnitude);
            if (direction.magnitude >= 0.1f && !isAttacking)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hero_attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            isAttacking = false;
        }
    }

    public void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // Debug.Log("Taken damage " + damage.ToString());
        healthImg.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth.ToString()} / {maxHealth.ToString()}";
        // animator.SetTrigger("Hurt");
        //move backward
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void Die() {
        gameObject.layer = 11;
        Debug.Log("Hero died");
        animator.SetBool("IsDead", true);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}
