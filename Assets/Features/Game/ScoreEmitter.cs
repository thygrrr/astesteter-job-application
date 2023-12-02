using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Game
{
    public class ScoreEmitter : DataChannelEmitter<IntChannel, int>, IOnDeath
    {
        [SerializeField] private int score = 0;

        public void OnDeath()
        {
            Emit(score);
        }
    }
}
