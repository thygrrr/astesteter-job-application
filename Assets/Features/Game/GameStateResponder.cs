using Channels.Concrete;
using Tiger.Events;

namespace Features.Game
{
    public class GameStateResponder : DataChannelResponder<GameStateChannel, GameState>
    {
    }
}
