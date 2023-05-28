using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
    // Event to register
    public VoidEventChannelSO Event;

    // Function to call when the Event is invoked
    public UnityEvent Callback;

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

	private void OnEventRaised()
	{
		if (Event != null)
			Callback.Invoke();
	}
}