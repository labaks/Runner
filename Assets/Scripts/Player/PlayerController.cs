using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] int currentHealth;
    public Transform canvas;
    Image healthImg;
    Text healthText;
    Animator animator;
    public GameObject damagePopup;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        healthImg = canvas.Find("HealthBarBg").Find("HealthBar").GetComponent<Image>();
        healthText = canvas.Find("HealthBarBg").Find("HealthBar").Find("HealthCount").GetComponent<Text>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // instantiateDamagePopup(damage);
        healthImg.fillAmount = (float)currentHealth / (float)maxHealth;
        healthText.text = $"{currentHealth.ToString()} / {maxHealth.ToString()}";
        // animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void instantiateDamagePopup(int damage)
    {
        Vector3 vector = new Vector3(-830f, 360f, 0);
        GameObject damageText = Instantiate(damagePopup, vector, Quaternion.identity, canvas);
        damageText.GetComponent<Text>().text = "-" + damage.ToString();
        Destroy(damageText.gameObject, 0.5f);
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
