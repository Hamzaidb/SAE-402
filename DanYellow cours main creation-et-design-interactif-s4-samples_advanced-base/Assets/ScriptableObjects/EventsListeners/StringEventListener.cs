using UnityEngine;
using UnityEngine.Events;

public class StringEventListener : MonoBehaviour
{
    // Event to register
    public StringEventChannelSO Event;

    // Function to call when the Event is invoked
    public UnityEvent<string> Callback;

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

	private void OnEventRaised(string value)
	{
		if (Event != null)
			Callback.Invoke(value);
	}
}