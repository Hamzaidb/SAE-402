using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Camera Shake Event", menuName = "ScriptableObjects/Events/CameraShakeEventChannelSO")]
public class CameraShakeEventChannelSO : ScriptableObject
{
    public UnityAction<ShakeTypeVariable> OnEventRaised;

	public void Raise(ShakeTypeVariable so)
	{
		// We wheck if someone is really listening to our event
		if (OnEventRaised != null) {
			OnEventRaised.Invoke(so);
		} else {
			Debug.LogWarning("An event of type " + GetType().Name + " was raised without any listener");
		}
	}
}
