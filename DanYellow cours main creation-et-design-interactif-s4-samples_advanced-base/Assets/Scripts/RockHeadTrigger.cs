using UnityEngine.Events;
using UnityEngine;

public class RockHeadTrigger : MonoBehaviour
{
    [HideInInspector]
    public GameObject sibling = null;

    public UnityEvent onTrigger;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == sibling) {
            onTrigger?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
