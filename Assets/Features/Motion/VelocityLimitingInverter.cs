using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Motion
{
    public class VelocityLimitingInverter : DataChannelEmitter<Vector3Channel, Vector3>
    {
        public float magnitude = 20f;
        public float lambda = 0.5f;
        

        private Vector3 _currentValue;
        private Vector3 _goalValue;
        private Vector3 _derivative;
        
        private void OnEnable()
        {
            _currentValue = channel.value;
            _goalValue = -1.0f * Vector3.ClampMagnitude(channel.value, magnitude);
        }

        private void Update()
        {
            _currentValue = Vector3.SmoothDamp(_currentValue, _goalValue, ref _derivative, lambda);
            Emit(_currentValue);
        }
    }
}
