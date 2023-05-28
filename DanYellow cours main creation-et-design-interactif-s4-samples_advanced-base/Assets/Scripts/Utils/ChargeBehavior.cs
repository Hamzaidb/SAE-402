using UnityEngine;
using System.Collections.Generic;

public class ChargeBehavior : MonoBehaviour
{
    [Header("Distance of sight")]
    public float range = 100;
    public LayerMask targetLayer;

    public float knockbackStrength = 2.5f;

    private Vector3 destination;

    private List<Vector3> listDirections = new List<Vector3>();
    private bool isAttacking;

    public SpriteRenderer spriteRenderer;

    private float checkTimer;
    public float checkDelay;
    public float speed;
    public Rigidbody2D rb;
    public Animator animator;

    public bool isFacingRight = true;

    [Header("Manage directions where the GameObject can looking for specific layers")]
    public bool checkRight = true;
    public bool checkLeft = true;
    public bool checkTop = true;
    public bool checkBottom = true;

    private float normalImpulseThreshold = 0;

    [Header("Shake effect")]
    public CameraShakeEventChannelSO onCrushSO;
    public ShakeTypeVariable shakeInfo;

    private bool isOnScreen = false;

    private bool attackWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        normalImpulseThreshold = (rb.mass * 1000) / 3000;
        if (checkLeft)
        {
            listDirections.Add(Vector3.left);
        }

        if (checkTop)
        {
            listDirections.Add(Vector3.up);
        }

        if (checkBottom)
        {
            listDirections.Add(Vector3.down);
        }

        if (checkRight)
        {
            listDirections.Add(Vector3.right);
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking && !attackWaiting)
        {
            spriteRenderer.color = Color.red;
            rb.AddForce(destination * speed, ForceMode2D.Impulse);
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForTarget();
        }

        if (animator != null)
        {
            animator.SetFloat("MoveDirectionX", Mathf.Abs(rb.velocity.x));
        }
    }

    private void CheckForTarget()
    {
        //Check if gameobject sees player in direction selected
        for (int i = 0; i < listDirections.Count; i++)
        {
            float offset = 0;

            if (isFacingRight)
            {
                offset = (listDirections[i].x > 0) ? range : (range / 2);
            }
            else
            {
                offset = (listDirections[i].x < 0) ? range : (range / 2);
            }

            Vector3 rayDirection = listDirections[i] * range;
            Vector3 startCast = transform.position;
            Vector3 endCast = transform.position + (rayDirection.normalized * offset);
            Debug.DrawLine(startCast, endCast, Color.green);

            RaycastHit2D hit = Physics2D.Linecast(startCast, endCast, targetLayer);

            if (hit.collider != null && !isAttacking && !attackWaiting)
            {
                destination = listDirections[i];
                isAttacking = true;
                checkTimer = 0;

                Flip();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        ContactPoint2D[] allContacts = new ContactPoint2D[other.contactCount];
        other.GetContacts(allContacts);

        foreach (ContactPoint2D contact in allContacts)
        {
            if
            (
                ((contact.normal.x > 0.5f && !isFacingRight) || (contact.normal.x < -0.5f && isFacingRight)) &&
                contact.normalImpulse > normalImpulseThreshold
            )
            {
                DetectCollision(other);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactPoint2D[] allContacts = new ContactPoint2D[other.contactCount];
        other.GetContacts(allContacts);

        foreach (ContactPoint2D contact in allContacts)
        {
            if (
                (contact.normal.x > 0.5f && !isFacingRight) || (contact.normal.x < -0.5f && isFacingRight)
                )
            {
                DetectCollision(other);
                if (other.gameObject.TryGetComponent<Knockback>(out Knockback knockback))
                {
                    Vector2 direction = (transform.position - other.gameObject.transform.position).normalized * -1f;
                    knockback.Knockbacked(direction, knockbackStrength);
                }
            }
        }
    }

    private void DetectCollision(Collision2D other)
    {
        animator.SetTrigger("IsHit");
        if (isOnScreen)
        {
            onCrushSO.Raise(shakeInfo);
        }
        Stop();
    }

    private void Stop()
    {
        isAttacking = false;
        rb.velocity = Vector2.zero;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void Flip()
    {
        if (
                (isFacingRight && destination == Vector3.left) ||
                (!isFacingRight && destination == Vector3.right)
            )
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    void OnBecameInvisible()
    {
        isOnScreen = false;
    }

    void OnBecameVisible()
    {
        isOnScreen = true;
    }
}
