using Channels.Concrete;
using Features.Game;
using Tweens;
using UnityEngine;

namespace Features.Player
{
    public class PlayerSpawnProcedure : GameStateEmitter
    {
        [SerializeField] private float delay = 1f;
        [SerializeField] private float duration = 0.5f;
        
        private void Start()
        {
            transform.localScale = Vector3.zero;
            var tween = new LocalScaleTween()
            {
                delay = delay,
                duration = duration,
                easeType = EaseType.BackOut,
                from = Vector3.zero,
                to = Vector3.one,
                onAdd = OnAdd,
                onEnd = OnEnd
            };
            
            gameObject.AddTween(tween);
        }

        private void OnAdd(TweenInstance<Transform, Vector3> instance)
        {
            Emit(GameState.Spawning);
        }

        private void OnEnd(TweenInstance<Transform, Vector3> instance)
        {
            Emit(GameState.Alive);
        }
    }
}
