//SPDX-License-Identifier: Unlicense

using Features.Game;
using Tiger.Events;
using UnityEngine;
using UnityEngine.Scripting;

namespace Channels.Concrete
{
    [CreateAssetMenu(menuName = "Event/GameState")]
    [Preserve]
    public class GameStateChannel : DataChannel<GameState>
    {
    }
}