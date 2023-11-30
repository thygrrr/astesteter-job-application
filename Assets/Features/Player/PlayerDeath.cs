//SPDX-License-Identifier: Unlicense

using Features.Game;

namespace Features.Player
{
    public class PlayerDeath : GameStateEmitter, IOnDeath
    {
        public void OnDeath()
        {
            Emit(GameState.Dying);            
        }
    }
}

