using Tiger.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Game
{
    public class GameStateButton : GameStateEmitter
    {
        [SerializeField]        
        private GameState destinationState = GameState.Spawning;
        [SerializeField]        
        private AudioEvent onClick;

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
            onClick.PlayOneShot();
            Emit(destinationState);
        }
    }
}
