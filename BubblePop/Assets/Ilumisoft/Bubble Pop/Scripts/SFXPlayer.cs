namespace Ilumisoft.BubblePop
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField]
        AudioClip selectSFX = null;

        [SerializeField]
        AudioClip popSFX = null;

        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayPopSFX()
        {
            PlayOneShot(popSFX);
        }

        public void PlaySelectSFX()
        {
            PlayOneShot(selectSFX);
        }

        void PlayOneShot(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}