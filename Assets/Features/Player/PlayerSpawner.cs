using Feature.Ui;
using Features.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Player
{
    using Log = Loggers.Create<PlayerSpawner>;
    public class PlayerSpawner : GameStateEmitter
    {
        [SerializeField]
        private GameObject playerPrefab;

        private GameObject _player;
    
        private GameInputActions _input;

        protected void Awake()
        {
            _input = new GameInputActions();
            _input.UI.Spawn.performed += SpawnPlayer;
            _input.UI.Enable();

            channel.Subscribe(OnExternalStateChange);
        }

        private void OnExternalStateChange(GameState state)
        {
            if (state is GameState.Epitaph or GameState.TitleScreen) _input.UI.Enable();
            else _input.UI.Disable();
        }


        protected void OnDestroy()
        {
            _input.UI.Disable();
        }

        
        private void SpawnPlayer(InputAction.CallbackContext ctx)
        {
            if (_player) return;
            Log.Info($"Spawn requested in state {channel.value}");
            
            _player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            _player.name = _player.name.Split("(Clone)")[0];
        }
    }
}
