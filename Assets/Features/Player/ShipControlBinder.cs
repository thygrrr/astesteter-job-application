//SPDX-License-Identifier: Unlicense

using System;
using Feature.Ui;
using Features.Game;
using UnityEngine;

namespace Features.Player
{
    using Log = Loggers.Create<ShipControlBinder>;

    public class ShipControlBinder : GameStateResponder
    {
        private GameInputActions _input;

        private void Awake()
        {
            Log.TagColor = Color.yellow;
            Log.Logger.filterLogType = LogType.Warning;
            _input = new GameInputActions();

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
            
            OnEvent(channel.value);
        }

        private void OnDestroy()
        {
            _input.Disable();
            foreach (var client in GetComponentsInChildren<GameInputActions.IFlightActions>())
            {
                Log.Info($"Unbinding FLIGHT ActionMap for {client}");
                _input.Flight.RemoveCallbacks(client);
            }

            foreach (var client in GetComponentsInChildren<GameInputActions.IWeaponActions>())
            {
                Log.Info($"Unbinding WEAPON ActionMap for {client}");
                _input.Weapon.RemoveCallbacks(client);
            }
        }

        protected override void OnEvent(GameState state)
        {
            switch (state)
            {
                case GameState.Spawning:
                case GameState.Alive:
                    _input.Enable();
                    break;
                
                default:
                    _input.Disable();
                    break;
            }
        }
    }
}
