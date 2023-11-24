using Tiger.Events;
using Tiger.Events.Concrete;
using Tiger.Swizzles;
using UnityEngine;

namespace Features.Rendering
{
    public class FollowLookPosition : DataChannelResponder<Vector3Channel, Vector3>, IHierarchicalUpdate
    {
        [SerializeField] 
        private Transform targetChild;

        [SerializeField] private float maxRadius = 40;
        [SerializeField] private Vector3 positionFactors = Vector3.one;
        [SerializeField] private float lookFactor = 0.5f;
    
        [SerializeField] 
        private float smoothLambda = 0.5f;

        private Vector3 _position;
        private Vector3 _positionGoal;
        private Vector3 _positionDerivative;

        protected override void OnEvent(Vector3 velocity)
        {
            _positionGoal = Vector3.ClampMagnitude(velocity, maxRadius);   
        }

        void IHierarchicalUpdate.HierarchicUpdate(float deltaTime)
        {
            _position = Vector3.SmoothDamp(_position, _positionGoal, ref _positionDerivative, smoothLambda);
            targetChild.localPosition = (_position._xyz() * positionFactors._xyz());

            targetChild.LookAt(_position * lookFactor, Vector3.up + Vector3.forward);
        }
    }
}
