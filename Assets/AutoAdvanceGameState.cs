using System.Collections;
using Features.Game;
using UnityEngine;

public class AutoAdvanceGameState : GameStateEmitter
{
    public float seconds = 3;

    private void OnEnable() => StartCoroutine(Advance());
    
    private IEnumerator Advance()
    {
        yield return new WaitForSeconds(seconds);
        Emit(GameState.Ready);
    }
}
