using Tiger.Events;
using UnityEngine;

namespace Channels.Concrete
{
    [CreateAssetMenu(menuName = "Event/GameState")]
    public class GameStateChannel : DataChannel<GameState>
    {
    }

    public enum GameState
    { 
        Menu,
        Spawning,
        Alive,
        Dead,
    }
}