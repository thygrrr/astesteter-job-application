using System.Collections;
using Channels.Concrete;
using UnityEngine;

namespace Features.Player
{
    public class DeathStateEmitter : MonoBehaviour
    {
        [SerializeField]
        private GameStateChannel channel;

        private void OnDestroy()
        {
            channel.Emit(GameState.Dead, this);
        }
    }
}
