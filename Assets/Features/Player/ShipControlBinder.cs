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

        protected override void OnEvent(GameState state)
        {
            switch (state)
            {
                case GameState.Alive:
                    _input.Enable();
                    break;
                
                case GameState.Dead:
                case GameState.Spawning:
                case GameState.Menu:
                default:
                    _input.Disable();
                    break;
            }
        }
    }
}
