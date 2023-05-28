using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Animator animator;
    public GameObject projectile;

    [Tooltip("From where the projectile will be shot")]
    public Transform firePoint;
    public SpriteRenderer spriteRenderer;

    [Range(0, 5)]
    public float timeDelayBetweenShots;

    [Tooltip("Warning time before first shot")]
    public float delayBeforeFirstShot;
    public int nbOfConsecutiveShots;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayAnimInterval(nbOfConsecutiveShots));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            StopAllCoroutines();
        }
    }

    private IEnumerator PlayAnimInterval(int nbIterations)
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(delayBeforeFirstShot);

        while (nbIterations > 0)
        {
            animator.Play("PlantAttack");
            nbIterations = nbIterations - 1;

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + timeDelayBetweenShots);
        }
    }

    // Better to call it in the timeline
    public void Shoot()
    {
        Instantiate(projectile, firePoint.position, firePoint.rotation);
    }
}
