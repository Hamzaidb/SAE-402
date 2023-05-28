using UnityEngine;
using System.Collections;

public class RockHead : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed;

    private Vector3 destination;

    public RockHeadTrigger[] listTriggers;
    public float delayBetweenMoves;

    private int currentIndex = 0;

    public Animator animator;
    private string lastAnimationPlayed = "";

    public CameraShakeEventChannelSO onCrushSO;
    public ShakeTypeVariable shakeInfo;

    private bool isOnScreen = false;

    // Value for which the go will be considered as crushed if it has contact with a RockHead
    private float maxImpulse = 1000;

    void Start()
    {
        EnableTriggers();
        SetTriggersSibling();
        StartCoroutine(GoToTrigger(listTriggers[currentIndex].transform.position));
    }

    void EnableTriggers()
    {
        for (int i = 0; i < listTriggers.Length; i++)
        {
            listTriggers[i].gameObject.SetActive(i == currentIndex);
        }
    }

    void SetTriggersSibling()
    {
        for (int i = 0; i < listTriggers.Length; i++)
        {
            listTriggers[i].sibling = gameObject;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(destination * speed, ForceMode2D.Impulse);
    }

    public void ChangeTrigger()
    {
        currentIndex = (currentIndex + 1) % listTriggers.Length;
        EnableTriggers();
        StartCoroutine(GoToTrigger(listTriggers[currentIndex].transform.position));
    }

    IEnumerator GoToTrigger(Vector2 dir)
    {
        yield return new WaitForSeconds(delayBetweenMoves);
        destination = -((Vector2) transform.position - dir).normalized;
        destination.x = Mathf.Round(destination.x);
        destination.y = Mathf.Round(destination.y);

        if (destination.x == 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        other.GetContacts(contacts);

        if (other.gameObject.CompareTag("Player"))
        {
            DetectCollision(other);
            if (contacts[0].normal.y < -0.5f)
            {
                other.gameObject.transform.parent = transform;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (rb.velocity == Vector2.zero)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                if (destination.y > 0 && lastAnimationPlayed != "HitTop")
                {
                    OnCrush("HitTop");
                }
                else if (destination.y < 0 && lastAnimationPlayed != "HitBottom")
                {
                    OnCrush("HitBottom");
                }
                if (destination.x > 0 && lastAnimationPlayed != "HitRight")
                {
                    OnCrush("HitRight");
                }
                else if (destination.x < 0 && lastAnimationPlayed != "HitLeft")
                {
                    OnCrush("HitLeft");
                }
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            DetectCollision(other);
        }
    }

    void OnCrush(string side)
    {
        animator.SetTrigger(side);
        lastAnimationPlayed = side;
        if (isOnScreen)
        {
            onCrushSO.Raise(shakeInfo);
        }
        EnableTriggers();
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = null;
        }
    }

    private void DetectCollision(Collision2D other)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[other.contactCount];
        other.GetContacts(contacts);

        foreach (ContactPoint2D contact in contacts)
        {
            if (
                (
                contact.normal.y > 0.5 ||
                contact.normal.y < -0.5 ||
                contact.normal.x < -0.5 ||
                contact.normal.x > 0.5
                ) &&
                contact.normalImpulse > maxImpulse &&
                other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health)
            )
            {
                other.gameObject.transform.parent = null;
                health.TakeDamage(float.MaxValue);
            }
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