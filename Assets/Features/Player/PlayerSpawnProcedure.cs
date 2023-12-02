//SPDX-License-Identifier: Unlicense
using Features.Game;
using Tiger.Audio;
using Tweens;
using UnityEngine;

namespace Features.Player
{
    public class PlayerSpawnProcedure : GameStateEmitter
    {
        [SerializeField] private float delay = 1f;
        [SerializeField] private float duration = 0.5f;
        
        [SerializeField] private AudioPool audioPool;
        [SerializeField] private AudioComposite spawnSound;
        
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
                onEnd = OnEnd
            };
            
            gameObject.AddTween(tween);
            
            if (!audioPool) audioPool = GetComponentInParent<AudioPool>();
            spawnSound.Play(audioPool);
        }
        
        private void OnEnd(TweenInstance<Transform, Vector3> instance)
        {
            Emit(GameState.Alive);
        }
    }
}
