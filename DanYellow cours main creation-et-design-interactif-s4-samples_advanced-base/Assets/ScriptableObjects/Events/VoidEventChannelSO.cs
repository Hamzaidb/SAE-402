using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Void Event", menuName = "ScriptableObjects/Events/VoidEventChannelSO")]
public class VoidEventChannelSO : ScriptableObject
{
    public UnityAction OnEventRaised;

	public void Raise()
	{
		// We wheck if someone is really listening to our event
		if (OnEventRaised != null) {
			OnEventRaised.Invoke();
		} else {
			Debug.LogWarning("Some gameobject raised");
		}
	}
}
