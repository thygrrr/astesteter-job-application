using Tiger.Audio;
using UnityEngine;

namespace Features.Game
{
    public class OnAwakePlayOneShotAudio : MonoBehaviour
    {
        [SerializeField] private AudioEvent audioEvent;
    
        private void Awake()
        {
            if (audioEvent) audioEvent.PlayOneShot(transform.position);
        }
    }
}
