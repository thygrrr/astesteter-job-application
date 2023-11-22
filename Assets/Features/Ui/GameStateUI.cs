using Channels.Concrete;
using Features.Game;
using Tiger.Util;

namespace Features.Ui
{
    using Log = Loggers.Create<GameStateUI>;
    
    public class GameStateUI : GameStateResponder
    {
        protected override void OnEvent(GameState state)
        {
            Log.Info($"Switching UI to {state}", this);
            ShowChildExclusive(state);
        }

        /// <remarks>
        /// This is a new pattern for me; I originally had a selector using PlayerInput.currentControlScheme
        /// It's definitely a cute experiment, but I like how it can decouple the UI from the game state 
        /// </remarks>
        private void ShowChildExclusive(GameState state)
        {
            foreach (var child in transform.Children())
            {
                child.gameObject.SetActive(child.name.Contains(state.ToString()));
            }
        }
    }
}

