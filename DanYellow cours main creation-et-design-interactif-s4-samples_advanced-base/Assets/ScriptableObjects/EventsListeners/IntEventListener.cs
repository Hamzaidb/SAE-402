using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int>{}

public class IntEventListener : MonoBehaviour
{
    // Event to register
    public IntEventChannelSO Event;

    // Function to call when the Event is invoked
    public UnityEvent<int> Callback;

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

	private void OnEventRaised(int value)
	{
		if (Event != null)
			Callback.Invoke(value);
	}
}