using Features.Game;
using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

public class ScoreEmitter : DataChannelEmitter<IntChannel, int>, IOnDeath
{
    [SerializeField] private int score = 0;

    public void OnDeath()
    {
        Emit(score);
    }
}
