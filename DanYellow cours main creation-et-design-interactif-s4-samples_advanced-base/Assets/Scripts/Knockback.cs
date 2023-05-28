using UnityEngine;
using System.Collections;

public class Knockback : MonoBehaviour
{
    public Rigidbody2D rb;

    public void Knockbacked(Vector3 direction, float strength)
    {
        rb.MovePosition(transform.position + direction * strength);
        StartCoroutine(DisableControls());
    }

    IEnumerator DisableControls()
    {
        if (TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.enabled = false;
            yield return new WaitForSeconds(0.20f);
            playerMovement.enabled = true;
        }
    }
}
