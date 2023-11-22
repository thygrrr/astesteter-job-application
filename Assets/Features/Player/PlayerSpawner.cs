using Channels.Concrete;
using Feature.Ui;
using Tiger.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Player
{
    public class PlayerSpawner : DataChannelResponder<GameStateChannel, GameState>
    {
        [SerializeField]
        private GameObject playerPrefab;

        private GameObject _player;
    
        private GameInputActions _input;
        
        protected override void AwakeOverride()
        {
            _input = new GameInputActions();
            _input.UI.Spawn.performed += SpawnPlayer;
        }

        private void OnEnable() => _input?.UI.Enable();
    
        private void OnDisable() => _input?.UI.Disable();

        private void SpawnPlayer(InputAction.CallbackContext ctx)
        {
            if (_player) return;
            _player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            _player.name = _player.name.Split("(Clone)")[0];
        }
    }
}
