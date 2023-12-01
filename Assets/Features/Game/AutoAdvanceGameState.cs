using System.Collections;
using UnityEngine;

namespace Features.Game
{
    public class AutoAdvanceGameState : GameStateEmitter
    {
        public ScoreBoard board;
        
        public GameState destinationState = GameState.Epitaph;
        public float seconds = 3;

        private void OnEnable() => StartCoroutine(Advance());
    
        private IEnumerator Advance()
        {
            yield return new WaitForSeconds(seconds);
            if (board.lives > 0)
            {
                Emit(destinationState);
            }
            else
            {
                Emit(GameState.GameOver);
            }
        }
    }
}
