using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    using Log = Loggers.Create<ProvideIntegration>;
    
    [DisallowMultipleComponent]
    public abstract class ProvideIntegration : DataChannelResponder<Vector3Channel, Vector3>
    {
        #region Public Properties / API

        public float3 finalVelocity => _velocity + _worldVelocity;

        public quaternion angular
        {
            get => _angular;
            set => _angular = math.normalizesafe(value); //sic, this is very cheap and saves us much heartache.
        }

        public float3 velocity
        {
            get => _velocity;
            set
            {
                if (math.abs(value.y) > float.Epsilon) Log.Warn($"Velocity has vertical component, {value}", this);
                _velocity = value;
                _velocity.y = 0;
            }
        }

        protected float3 _velocity;
        protected float3 _worldVelocity;
        protected quaternion _angular = quaternion.identity;
        
        #endregion
        
        public void InstantiateWithVelocity(Vector3 worldPosition, Quaternion worldRotation, Transform parent, Vector3 ownVelocity)
        {
            var instance = Instantiate(gameObject, worldPosition, worldRotation, parent).GetComponent<ProvideIntegration>();
            instance._velocity = ownVelocity;
        }
    }
}
