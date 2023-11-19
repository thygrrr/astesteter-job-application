//SPDX-License-Identifier: Unlicense
using Feature.Ui;
using UnityEngine.InputSystem;

namespace Features.Player
{
    using Log = Loggers.Create<ShipControls>;
    
    public class ShipControls : PlayerInput
    {
        private GameInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new GameInputActions();
        }
    }
}
