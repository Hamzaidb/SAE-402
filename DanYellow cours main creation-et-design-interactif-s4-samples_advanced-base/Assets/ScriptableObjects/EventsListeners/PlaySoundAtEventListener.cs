using UnityEngine;
using UnityEngine.Events;

public class PlaySoundAtEventListener : MonoBehaviour
{
    // Event to register
    public PlaySoundAtEventChannelSO Event;

    // Function to call when the Event is invoked
    public UnityEvent<AudioClip, Vector3> Callback;

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

	private void OnEventRaised(AudioClip sound, Vector3 position)
	{
		if (Event != null)
			Callback.Invoke(sound, position);
	}
}