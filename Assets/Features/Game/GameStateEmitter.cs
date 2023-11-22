//SPDX-License-Identifier: Unlicense

using Channels.Concrete;
using Tiger.Events;

namespace Features.Game
{
    public abstract class GameStateEmitter : DataChannelEmitter<GameStateChannel, GameState>
    {
    }
}
