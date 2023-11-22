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
            Log.TagColor = Color.yellow;
            _input = new GameInputActions();
        }

        private void Start()
        {
            foreach (var client in GetComponentsInChildren<GameInputActions.IFlightActions>())
            {
                Log.Info($"Binding FLIGHT ActionMap for {client}");
                _input.Flight.AddCallbacks(client);
            }

            foreach (var client in GetComponentsInChildren<GameInputActions.IWeaponActions>())
            {
                Log.Info($"Binding WEAPON ActionMap for {client}");
                _input.Weapon.AddCallbacks(client);
            }
        }

        private void OnEnable() => _input?.Enable();
        private void OnDisable() => _input?.Disable();

        #endregion
    }
}
