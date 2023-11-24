//SPDX-License-Identifier: Unlicense
using Tweens;
using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Space
{
    using Log = Loggers.Create<SunlightDependentSkyboxExposure>;

    public class SunlightDependentSkyboxExposure : DataChannelResponder<BoolChannel, bool>
    {
        [Header("Skybox Exposure")] [SerializeField]
        private float litExposure = 1f;

        [SerializeField] private float darkExposure = 2f;

        [Header("Ambient Light")] [SerializeField] [ColorUsage(false, true)]
        private Color litAmbientLight;

        [SerializeField] [ColorUsage(false, true)]
        private Color darkAmbientLight;

        private static readonly int exposure = Shader.PropertyToID("_Exposure");

        private float _value = 1;

        protected override void OnEvent(bool sunshine)
        {
            gameObject.CancelTweens();
            gameObject.AddTween(new FloatTween
            {
                duration = 1,
                from = _value,
                to = sunshine ? 1 : 0,
                onUpdate = OnTweenUpdate
            });
        }

        private void OnTweenUpdate(TweenInstance<Transform, float> _, float value)
        {
            _value = value;
            SetAmbientAndExposure();
        }

        private void SetAmbientAndExposure()
        {
            RenderSettings.ambientSkyColor = Color.Lerp(darkAmbientLight, litAmbientLight, _value);
            
            if (!RenderSettings.skybox) return;
            RenderSettings.skybox.SetFloat(exposure, math.lerp(darkExposure, litExposure, _value));
        }

        protected override void OnDisableOverride()
        {
            gameObject.CancelTweens();
            _value = 1;
            SetAmbientAndExposure();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (litAmbientLight == default) litAmbientLight = RenderSettings.ambientSkyColor;
            else RenderSettings.ambientSkyColor = litAmbientLight;

            if (!RenderSettings.skybox) return;
            RenderSettings.skybox.SetFloat(exposure, litExposure);
        }
    }
}
