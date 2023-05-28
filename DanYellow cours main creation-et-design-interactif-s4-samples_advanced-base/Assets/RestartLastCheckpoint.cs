using UnityEngine;
using UnityEngine.UI;

public class RestartLastCheckpointButton : MonoBehaviour
{
    public CurrentSceneManager onRestartLastCheckpoint;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(RestartLastCheckpoint);
    }

    private void RestartLastCheckpoint()
    {
        onRestartLastCheckpoint.RestartLastCheckpoint();
    }
}

