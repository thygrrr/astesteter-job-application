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

        private void ShowChildExclusive(GameState state)
        {
            foreach (var child in transform.Children())
            {
                child.gameObject.SetActive(child.name == state.ToString());
            }
        }
    }
}

