using UnityEngine.UI;

namespace Features.Game
{
    public class GameStateButton : GameStateEmitter
    {
        public GameState destinationState = GameState.Spawning;
        private Button _button;
    
        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Advance);
        }
    
        private void OnDisable()
        {
            _button.onClick.RemoveListener(Advance);
        }

        private void Advance()
        {
            Emit(destinationState);
        }
    }
}
