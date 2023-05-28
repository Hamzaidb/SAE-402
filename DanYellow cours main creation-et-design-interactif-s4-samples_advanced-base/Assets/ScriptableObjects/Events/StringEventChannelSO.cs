using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New String Event", menuName = "ScriptableObjects/Events/StringEventChannelSO")]
public class StringEventChannelSO : ScriptableObject
{
    public UnityAction<string> OnEventRaised;

	public void Raise(string value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
