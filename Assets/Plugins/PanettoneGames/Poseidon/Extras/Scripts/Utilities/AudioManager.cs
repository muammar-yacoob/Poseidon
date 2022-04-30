using UnityEngine;

namespace PanettoneGames.Poseidon.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private AudioSource audioSource;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            PooledShootingBehaviour.OnFire += Handle_OnFile;
        }

        private void OnDestroy() => PooledShootingBehaviour.OnFire -= Handle_OnFile;

        private void Handle_OnFile(AudioClip sfx) => audioSource?.PlayOneShot(sfx);
    }
}