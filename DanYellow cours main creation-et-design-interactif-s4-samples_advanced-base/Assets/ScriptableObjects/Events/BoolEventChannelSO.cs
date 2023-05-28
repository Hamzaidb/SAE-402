using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Bool Event", menuName = "ScriptableObjects/Events/BoolEventChannelSO")]
public class BoolEventChannelSO : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;

	public void Raise(bool value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
