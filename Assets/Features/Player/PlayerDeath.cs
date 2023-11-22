//SPDX-License-Identifier: Unlicense

using Features.Game;

namespace Features.Player
{
    public class DeathStateEmitter : GameStateEmitter
    {
        private void OnDestroy()
        {
            Emit(GameState.Dead);
        }
    }
}
