//SPDX-License-Identifier: Unlicense

using Tweens;
using UnityEngine;

namespace Features.Game
{
    public class DamageTaker : Killable
    {
        [SerializeField] private int hitPoints = 1;

        public void ApplyDamage(int damage)
        {
            hitPoints -= damage;
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
                from = Color.white,
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