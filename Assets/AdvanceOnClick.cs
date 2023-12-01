using Channels.Concrete;
using Feature.Ui;
using Features.Game;
using Tiger.Events;
using UnityEngine.InputSystem;

public class AdvanceOnClick : DataChannelEmitter<GameStateChannel, GameState>
{
    private GameInputActions _input;

    private void Awake()
    {
        _input = new GameInputActions();
        _input.UI.Spawn.performed += GoToMenu;
    }

    private void GoToMenu(InputAction.CallbackContext obj)
    {
        Emit(GameState.TitleScreen);
    }

    protected void OnEnable()
    {
        _input.UI.Enable();
    }

    private void OnDisable()
    {
        _input.UI.Disable();
        ;
    }
}
