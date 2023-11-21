using Tiger.Events;
using Tiger.Events.Concrete;
using Tweens;
using UnityEngine;

namespace Features.Space
{
    [RequireComponent(typeof(Light))]
    public class SunlightDependentLight : DataChannelResponder<BoolChannel, bool>
    {
        private bool _state = true;
        private Light _light;
        private float _litIntensity;

        private void Start()
        {
            _light = GetComponent<Light>();
            _litIntensity = _light.intensity;
        }

        protected override void OnEvent(bool sunshine)
        {
            if (_state == sunshine) return;
            _state = sunshine;
            
            var tween = new LightIntensityTween
            {
                easeType = EaseType.Linear,
                duration = 1.5f,
                from = _light.intensity,
                to = sunshine ? _litIntensity : 0f,
            };
            _light.gameObject.CancelTweens();
            _light.gameObject.AddTween(tween);
        }

        private void OnDisable()
        {
            _light.gameObject.CancelTweens();
            _light.intensity = _litIntensity;
        }
    }
}
