using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] int currentHealth;

    public Image healthImg;
    public Text healthText;
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthImg.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth.ToString()} / {maxHealth.ToString()}";
        // animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.layer = 11;
        Debug.Log("Hero died");
        animator.SetBool("IsDead", true);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}
