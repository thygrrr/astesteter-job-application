using Tiger.Audio;
using UnityEngine;

namespace Features.Game
{
    public class OnDeathPlayOneShotAudio : MonoBehaviour, IOnDeath
    {
        [Header("Audio Event / Sound Effect to play as 1-Shot")]
        [SerializeField] private AudioEvent audioEvent;

        public void OnDeath()
        {
            if (audioEvent) audioEvent.PlayOneShot(transform.position);   
        }
    }
}
