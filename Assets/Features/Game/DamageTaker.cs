//SPDX-License-Identifier: Unlicense

using Tweens;
using UnityEngine;

namespace Features.Game
{
    using Log = Loggers.Create<DamageTaker>;
    
    public class DamageTaker : Killable
    {
        [SerializeField] private float hitPoints = 1;
        [SerializeField] private float spawnInvulnerabilitySeconds = 0.5f;

        private float _invulnerableUntil;
        
        private void Start()
        {
            _invulnerableUntil = Time.time + spawnInvulnerabilitySeconds;
        }

        public void ApplyDamage(float damage)
        {
            if (Time.time > _invulnerableUntil) hitPoints -= damage;
            
            //Still PRETEND to take damage.
            if (hitPoints <= 0) Die(); else HitFeedback();
        }

        #region Crude Hit Feedback Implementation

        private MeshRenderer _renderer;
        private static readonly int emission = Shader.PropertyToID("_Emission");
        private ColorTween _tween;

        private void Awake()
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
            _tween = new ColorTween
            {
                from = Color.red * 1.25f,
                to = Color.black,
                duration = 0.2f,
                onUpdate = (_, value) => _renderer.material.SetColor(emission, value)
            };
        }

        private void HitFeedback()
        {
            gameObject.CancelTweens();
            gameObject.AddTween(_tween);
        }

        #endregion
    }
}