using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Tooltip("Define where the player will spawn if there is an issue"), ReadOnlyInspector]
    public Vector3 currentSpawnPosition;

    [Tooltip("Define where the player started the level"), ReadOnlyInspector]
    public Vector3 initialSpawnPosition;

    public VoidEventChannelSO onRestartLastCheckpoint;

    private void Awake()
    {     
        currentSpawnPosition = gameObject.transform.position;
        initialSpawnPosition = gameObject.transform.position;
    }

    private void OnEnable() {
        onRestartLastCheckpoint.OnEventRaised += MoveToLastCheckPoint;
    }

    private void MoveToLastCheckPoint() {
        gameObject.transform.position = currentSpawnPosition;
    }

    private void OnDisable() {
        onRestartLastCheckpoint.OnEventRaised -= MoveToLastCheckPoint;
    }
}
