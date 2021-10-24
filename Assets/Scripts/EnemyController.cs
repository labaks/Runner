using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public Transform body;
    public Transform head;
    public Image health;
    public Text healthText;

    public LayerMask hero;
    public GameObject hitParticles;
    private GameObject damageText;
    public GameObject damagePopup;

    public float hearingRange = 10f;
    public float attackRange = 1f;
    public float speed = 4f;

    public float maxHealth = 100;
    public int attackDamage = 10;
    bool isDead = false;
    GameObject hit;
    Transform canvas;

    [SerializeField] float currentHealth;
    [SerializeField] bool canAttack = true;
    void Start()
    {
        currentHealth = maxHealth;
        health.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth.ToString()} / {maxHealth.ToString()}";
        canvas = health.transform.parent.parent;
        canvas.forward = -Camera.main.transform.forward;
    }

    void Update()
    {
        if (!isDead)
        {
            canvas.forward = -Camera.main.transform.forward;
            Collider[] hearHero = Physics.OverlapSphere(head.position, hearingRange, hero, QueryTriggerInteraction.Ignore);
            if (hearHero.Length > 0)
            {
                canAttack = true;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f) canAttack = false;
                Transform hero = hearHero[0].transform;
                transform.LookAt(hero);
                if (Vector3.Distance(transform.position, hero.position) <= attackRange)
                {
                    if (canAttack)
                    {
                        animator.SetBool("IsMoving", false);
                        Attack(hero);
                    }
                }
                else
                {
                    GoToHero();
                }
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
    }

    public void Attack(Transform target)
    {
        animator.SetTrigger("Attack");
        target.GetComponent<PlayerController>().TakeDamage(attackDamage);
        canAttack = false;
    }

    public void GoToHero()
    {
        animator.SetBool("IsMoving", true);
        controller.Move(transform.forward * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        canAttack = false;
        currentHealth -= damage;
        instantiateDamagePopup(damage);
        health.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth.ToString()} / {maxHealth.ToString()}";
        animator.SetTrigger("Hurt");
        // hit = Instantiate(hitParticles, transform.position + Vector3.up, Quaternion.identity, transform);
        //move backward
        Hurted();
        if (currentHealth <= 0)
        {
            Die();
        }
        Invoke("Normalize", 0.5f);
    }

    public void instantiateDamagePopup(int damage)
    {
        damageText = Instantiate(damagePopup, transform.position + Vector3.up, Quaternion.identity, health.transform.parent.parent);
        damageText.GetComponent<Text>().text = "-" + damage.ToString();
    }

    void Die()
    {
        Debug.Log("Enemy died");
        isDead = true;
        animator.SetBool("IsDead", isDead);
        canvas.gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        // this.enabled = false;
    }

    void Normalize()
    {
        body.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
        head.GetComponent<MeshRenderer>().material.color = Color.white;
        // Destroy(hit.gameObject);
        Destroy(damageText.gameObject);
    }

    void Hurted()
    {
        head.GetComponent<MeshRenderer>().material.color = Color.red;
        body.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
    }

    private void OnDrawGizmosSelected()
    {
        if (head == null) return;
        Gizmos.DrawWireSphere(head.position, hearingRange);
        Gizmos.DrawWireSphere(head.position, attackRange);
    }
}

