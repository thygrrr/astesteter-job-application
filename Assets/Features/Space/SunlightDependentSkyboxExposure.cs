using Tweens;
using Tiger.Events;
using Tiger.Events.Concrete;
using Tweens.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Space
{
    using Log = Loggers.Create<SunlightDependentSkyboxExposure>;

    public class SunlightDependentSkyboxExposure : DataChannelResponder<BoolChannel, bool>
    {
        [SerializeField] private float litExposure = 1f;
        [SerializeField] private float darkExposure = 2f;

        private static Material skybox => RenderSettings.skybox;
        
        private static readonly int exposure = Shader.PropertyToID("_Exposure");

        protected override void OnEvent(bool sunshine)
        {
            gameObject.CancelTweens();
            gameObject.AddTween(new FloatTween
            {
                duration = 1,
                from = skybox.GetFloat(exposure),
                to = sunshine ? litExposure : darkExposure,
                onUpdate = (_, value) => skybox.SetFloat(exposure, value) 
            });
        }
        
        private void OnDisable()
        {
            gameObject.CancelTweens();
            skybox.SetFloat(exposure, litExposure);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!skybox) Log.Error("Skybox is not set\n(check lighting settings for the scene this object lives in).", this);
        }
    }
}
