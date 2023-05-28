using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public ParticleSystem particles;
    public StringEventChannelSO OnLevelEnded;
    public PlaySoundAtEventChannelSO sfxAudioChannel;
    public string nextLevel;
    public AudioClip audioClip;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasBeenTriggered)
        {
            particles.Play();
            hasBeenTriggered = true;
            sfxAudioChannel.Raise(audioClip, transform.position);
            if (nextLevel != null)
            {
                OnLevelEnded.Raise(nextLevel);
            }
        }
    }
}
