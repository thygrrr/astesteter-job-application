using Channels.Concrete;
using Tiger.Events;
using UnityEngine;

namespace Features.Rendering
{
    public class VelocityLookAhead : DataChannelResponder<Vector3Channel, Vector3>, IHierarchicalUpdate
    {
        [SerializeField] private Transform targetChild;

        [SerializeField] private Vector3 positionFactors = Vector3.one * -5;
        [SerializeField] private float curveExponent = .5f;
        [SerializeField] private float speedScale = 100f;
        [SerializeField] private float smoothLambda = 0.5f;

        private Vector3 _position;
        private Vector3 _positionGoal;
        private Vector3 _positionDerivative;
        
        protected override void OnEvent(Vector3 velocity)
        {
            var proportional = Vector3.ClampMagnitude(velocity,  speedScale) / speedScale;
            var magnitude = Mathf.Pow(proportional.magnitude, curveExponent);
            _positionGoal = proportional * magnitude;
            _positionGoal.Scale(positionFactors);
        }

        void IHierarchicalUpdate.HierarchicUpdate(float deltaTime)
        {
            _position = Vector3.SmoothDamp(_position, _positionGoal, ref _positionDerivative, smoothLambda);
            targetChild.localPosition = _position;
        }
    }
}
