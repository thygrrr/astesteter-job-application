using System.Collections;
using UnityEngine;

namespace Features.Game
{
    public class AutoAdvanceGameState : GameStateEmitter
    {
        public GameState destinationState = GameState.Ready;
        public float seconds = 3;

        private void OnEnable() => StartCoroutine(Advance());
    
        private IEnumerator Advance()
        {
            yield return new WaitForSeconds(seconds);
            Emit(destinationState);
        }
    }
}
