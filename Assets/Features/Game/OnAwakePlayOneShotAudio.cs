using Tiger.Audio;
using UnityEngine;

namespace Features.Game
{
    public class OnAwakePlayOneShotAudio : MonoBehaviour
    {
        [SerializeField] private AudioEvent audioEvent;
    
        private void Awake()
        {
            audioEvent.PlayOneShot(transform.position);
        }
    }
}
