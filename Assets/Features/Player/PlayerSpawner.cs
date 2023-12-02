using Features.Game;
using UnityEngine;

namespace Features.Player
{
    using Log = Loggers.Create<PlayerSpawner>;
    public class PlayerSpawner : GameStateEmitter
    {
        [SerializeField]
        private GameObject playerPrefab;
        
        private GameObject _player;
    
        protected void Awake()
        {
            channel.Subscribe(OnExternalStateChange);
        }

        private void OnExternalStateChange(GameState state)
        {
            if (state is GameState.Spawning)
            {
                SpawnPlayer();
            }
        }
        
        
        private void SpawnPlayer()
        {
            _player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            _player.name = _player.name.Split("(Clone)")[0];
        }
    }
}
