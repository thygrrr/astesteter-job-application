using Channels.Concrete;
using Feature.Ui;
using Features.Game;
using Tiger.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Player
{
    public class PlayerSpawner : DataChannelEmitter<GameStateChannel, GameState>
    {
        [SerializeField]
        private GameObject playerPrefab;

        private GameObject _player;
    
        private GameInputActions _input;
        
        protected void Awake()
        {
            _input = new GameInputActions();
            _input.UI.Spawn.performed += SpawnPlayer;
        }

        protected void OnEnable() => _input?.UI.Enable();

        protected void OnDisable() => _input?.UI.Disable();

        private void SpawnPlayer(InputAction.CallbackContext ctx)
        {
            if (_player) return;
            _player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            _player.name = _player.name.Split("(Clone)")[0];
            Emit(GameState.Spawning);
        }
    }
}
