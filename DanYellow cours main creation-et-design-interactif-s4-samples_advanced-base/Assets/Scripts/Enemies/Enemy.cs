using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Tooltip("How high will jump the player on the top of the enemy")]
    public float bounce = 10f;
    public FloatVariable maxHealth;

    [ReadOnlyInspector]
    public float currentHealth = 0f;

    public SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;
    public Animator animator;

    // List of contact points when something collides with that GameObject
    private ContactPoint2D[] contacts = new ContactPoint2D[1];

    [Header("Components to disable after specific event. E.g. : death")]
    public Behaviour[] listComponents;

    private float bounceFactorOnDeath = 0.15f;

    private void Start()
    {
        // If no max health is defined then the enemy heath is 1
        currentHealth = maxHealth?.CurrentValue ?? 1f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentHealth <= 0) return;

        other.GetContacts(contacts);

        if (
            other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth) &&
            other.gameObject.CompareTag("Player") &&
            contacts[0].normal.y > -0.5f
            )
        {
            playerHealth.TakeDamage(1f);
        }

        if (other.gameObject.CompareTag("Player") && contacts[0].normal.y < -0.5f)
        {
            StartCoroutine(TakeDamage(1f));
            Vector2 bounceForce = Vector2.up * bounce;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(bounceForce, ForceMode2D.Impulse);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    IEnumerator TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (animator)
        {
            animator.SetTrigger("IsHit");
        }
        else
        {
            UnityEngine.Color hitColor = new UnityEngine.Color(0.8207547f, 0.8207547f, 0.8207547f);
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    private void Die()
    {
        foreach (Behaviour component in listComponents)
        {
            component.enabled = false;
        }

        foreach (Collider2D collider in gameObject.GetComponentsInChildren<Collider2D>()) {
            collider.enabled = false;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        rb.velocity = Vector2.zero;
        Vector2 bounceForce = Vector2.up * (rb.mass * bounceFactorOnDeath);
        rb.AddForce(bounceForce, ForceMode2D.Impulse);

        gameObject.transform.Rotate(0f, 0f, 80f);
    }

    void OnBecameInvisible()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject, 0.15f);
        }
    }
}
