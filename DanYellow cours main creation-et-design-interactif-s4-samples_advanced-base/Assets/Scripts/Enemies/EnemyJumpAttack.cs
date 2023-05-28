using UnityEngine;
using System.Collections;

public class EnemyJumpAttack : MonoBehaviour
{
    public EnemyPatrol enemyPatrol;
    public Animator animator;
    public Rigidbody2D rb;
    public float bounceHeight;
    public LayerMask targetLayer;

    private Collider2D target;
    public Vector2 sightArea;
    public Vector2 sightAreaOffset;
    public float delayBeforeJump = 0.75f;

    private int moveDirection = 1;

    [Tooltip("Position checks")]
    public LayerMask listGroundLayers;
    public Transform groundCheck;
    public float groundCheckRadius;

    private ContactPoint2D[] contacts = new ContactPoint2D[1];

    private GameObject lastHit;
    private Vector3 sightOffset = Vector3.zero;

    private bool isGrounded;
    private bool canAttack = true;

    [Tooltip("How long it takes to reach the target")]
    public float jumpTime = 1.25f;

    private void Update()
    {
        if (enemyPatrol != null)
        {
            moveDirection = (enemyPatrol.isFacingRight == true) ? 1 : -1;
            enemyPatrol.enabled = (!target && isGrounded && GetComponent<Renderer>().isVisible);
        }

        sightOffset = ((Vector3)sightAreaOffset * moveDirection);

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!enemyPatrol.enabled && canAttack && target != null)
            {
                StartCoroutine(JumpAttack(target.gameObject.transform));
            }
        }

        if (
            target != null &&
            (
                (target.gameObject.transform.position.x - transform.position.x < 0 && enemyPatrol.isFacingRight) ||
                (target.gameObject.transform.position.x - transform.position.x > 0 && !enemyPatrol.isFacingRight)
            )
        )
        {
            StartCoroutine(enemyPatrol.Flip());
        }

        animator.SetFloat("MoveDirectionX", rb.velocity.x);
        animator.SetFloat("MoveDirectionY", rb.velocity.y);
    }
    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        target = Physics2D.OverlapBox(transform.position + sightOffset, sightArea, 0, targetLayer);

        if (!enemyPatrol.enabled && canAttack && target != null && isGrounded && rb.velocity.y == 0)
        {
            canAttack = false;
            StartCoroutine(JumpAttack(target.gameObject.transform));
        }
    }

    IEnumerator JumpAttack(Transform target)
    {
        yield return new WaitForSeconds(delayBeforeJump);

        Vector2 targetVel = CalculateTrajectoryVelocity(transform.position, target.position, jumpTime);
        rb.velocity = targetVel;

        canAttack = true;
        lastHit = null;
    }

    // Source : https://answers.unity.com/questions/1286132/how-can-i-solve-ballistic-angle-and-velocity-to-hi.html
    private Vector2 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics2D.gravity.y * Mathf.Pow(t, 2)) / t;

        return new Vector2(vx, vy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + sightOffset, sightArea);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, listGroundLayers);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.GetContacts(contacts);
        if (lastHit != other.gameObject)
        {
            lastHit = other.gameObject;
            canAttack = true;
        }

        if (
            contacts[0].normal.y > 0.5f &&
            other.gameObject.CompareTag("Player")
        )
        {
            Vector2 originVector = new Vector2(0.15f * moveDirection, 1);
            Vector2 bounceForce = originVector * bounceHeight * rb.mass;
            rb.AddForce(bounceForce, ForceMode2D.Impulse);
        }
    }
    
    public void OnDeath() {
        StopAllCoroutines();
    }
}
