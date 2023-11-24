using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Motion
{
    public class VelocityLimiter : DataChannelEmitter<Vector3Channel, Vector3>
    {
        public float magnitude = 20f;
        public float lambda = 0.5f;
        private float _derivative;

        private void Update()
        {
            var smoothMagnitude = Mathf.SmoothDamp(channel.value.magnitude, magnitude, ref _derivative, lambda);
            var limited = Vector3.ClampMagnitude(channel.value, smoothMagnitude);
        
            Emit(limited);
        }
    }
}
