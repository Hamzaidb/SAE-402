using UnityEngine;
using UnityEngine.Events;

public class CameraShakeEventListener : MonoBehaviour
{
    // Event to register
    public CameraShakeEventChannelSO Event;

    // Function to call when the Event is invoked
    public UnityEvent<ShakeTypeVariable> Callback;

    private void OnEnable()
	{
		if (Event != null)
			Event.OnEventRaised += OnEventRaised;
	}

	private void OnDisable()
	{
		if (Event != null)
			Event.OnEventRaised -= OnEventRaised;
	}

	private void OnEventRaised(ShakeTypeVariable so)
	{
		if (Event != null)
			Callback.Invoke(so);
	}
}