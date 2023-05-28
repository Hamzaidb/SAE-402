using UnityEngine;

public class EnemySplitting : MonoBehaviour
{
    public Animator animator;

    [Tooltip("Gameobject to instantiate when the enemy is hit")]
    public GameObject split;

    [Tooltip("Number of items created after being hit")]
    public int nbOfSplit;

    private bool hasSplitted = false;

    public Vector2 targetVector = Vector2.zero;

    public Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        other.GetContacts(contacts);

        if (other.gameObject.CompareTag("Player") && contacts[0].normal.y < -0.5f)
        {
            animator.SetTrigger("IsHit");
            Split();
        }
    }

    [ContextMenu("Split")]
    private void Split()
    {
        Quaternion angles;
        Vector3 position = transform.position;
        float posXDelta = 1.5f;
        if (!hasSplitted && split != null)
        {
            hasSplitted = true;
            for (var i = 0; i < nbOfSplit; i++)
            {
                bool willFacingRight = i % 2 != 0;

                angles = willFacingRight ? Quaternion.Euler(0f, -180f, 0f) : Quaternion.Euler(0f, 0f, 0f);

                Vector3 randomPosition = new Vector3(
                    Random.Range(position.x - posXDelta, position.x + posXDelta),
                    position.y,
                    position.z
                );

                GameObject child = Instantiate(split, randomPosition, angles);
                child.GetComponent<EnemyPatrol>().isFacingRight = willFacingRight;
            }
        }
    }
}
