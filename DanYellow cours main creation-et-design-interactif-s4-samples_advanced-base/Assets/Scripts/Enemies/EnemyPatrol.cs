
using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;
    public float speed;

    public bool isFacingRight = false;

    private bool isIdle = true;

    private float idleTime;

    [Tooltip("Define how long the enemy will walk")]
    public float walkTime = 5f;

    private Vector3 offset;

    private Vector3 lastKnownPosition = Vector3.zero;

    public bool hasCollisionWithObstacle;
    [UnityEngine.Serialization.FormerlySerializedAs("groundCheckRadius")]
    public float obstacleCheckRadius = 0.25f;
    private bool isFlipping = false;

    public LayerMask obstacleLayersMask;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        enabled = false;
    
    }

    private void Start()
    {
        offset = new Vector3(sr.bounds.extents.x * (isFacingRight ? -1 : 1), sr.bounds.extents.y, 0);
        idleTime = Mathf.Round(Random.Range(0, 3.5f));
        StartCoroutine(ChangeState());

        InvokeRepeating(nameof(UpdateLastKnownPosition), 3.0f, 3f);
    }

    void UpdateLastKnownPosition()
    {
        if (lastKnownPosition == transform.position && !isFlipping && (rb.velocity.y > -0.1f && rb.velocity.y < 0.1f))
        {
            StartCoroutine(Flip());
        }
        lastKnownPosition = transform.position;
    }

    private void Update()
    {
        if (isIdle)
        {
            Idle();
        }
        else
        {
            Move();
        }
        animator.SetFloat("MoveDirectionX", Mathf.Abs(rb.velocity.x));
    }

    private void FixedUpdate()
    {
        hasCollisionWithObstacle = HasCollisionWithObstacle();
        Vector3 startCast = transform.position - new Vector3(offset.x, 0, 0); ;
        Vector3 endCast = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * 0.9f;
        Debug.DrawLine(startCast, endCast, Color.green);

        RaycastHit2D hitObstacle = Physics2D.Linecast(startCast, endCast, obstacleLayersMask);
        if ((hitObstacle.collider != null || !hasCollisionWithObstacle))
        {
            StartCoroutine(Flip());
        }
    }

    IEnumerator ChangeState()
    {
        while (true)
        {
            // Enemy will walk during X seconds...
            isIdle = false;
            yield return new WaitForSeconds(walkTime);

            // ...then wait during X seconds...
            isIdle = true;
            yield return new WaitForSeconds(idleTime);
        }
    }

    private void Idle()
    {
        rb.velocity = Vector2.zero;
    }

    private void Move()
    {
        if (isFacingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    public bool HasCollisionWithObstacle()
    { 
        return Physics2D.OverlapCircle(transform.position - offset, obstacleCheckRadius, obstacleLayersMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - offset, obstacleCheckRadius);
    }

    public IEnumerator Flip()
    {
        offset.x *= -1;
        isFlipping = true;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        lastKnownPosition = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        isFlipping = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        // We stop the enemy when is not visible or else
        // it might continue to run but whoen be able to change direction
        Idle();
        enabled = false;
    }
}