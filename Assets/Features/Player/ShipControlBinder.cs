//SPDX-License-Identifier: Unlicense
using Feature.Ui;
using UnityEngine;

namespace Features.Player
{
    using Log = Loggers.Create<ShipControlBinder>;

    public class ShipControlBinder : MonoBehaviour
    {
        private GameInputActions _input;
        
        #region Unity Events

        private void Awake()
        {
            _input = new GameInputActions();

            var clients = GetComponentsInChildren<GameInputActions.IPlayerActions>();
            foreach (var client in clients)
            {
                Log.Info($"Binding action map for {client}");
                _input.Player.AddCallbacks(client);
            }
        }

        private void OnEnable() => _input.Player.Enable();
        private void OnDisable() => _input.Player.Disable();

        #endregion
    }
}
