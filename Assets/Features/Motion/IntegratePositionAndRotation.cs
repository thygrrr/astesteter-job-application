//SPDX-License-Identifier: Unlicense

using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    using Log = Loggers.Create<IntegratePositionAndRotation>;

    public class IntegratePositionAndRotation : DataChannelResponder<Vector3Channel, Vector3>
    {
        [Header("World-Relative Motion")] [SerializeField]
        private float velocityScale = 1;

        [SerializeField] private float spawnInheritVelocityFactor = 1;

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

        #endregion

        #region State Data
        
        private float3 _velocity;
        private float3 _worldVelocity;
        private quaternion _angular = quaternion.identity;
        
        protected override void OnEvent(Vector3 data) => _worldVelocity = data;
        
        #endregion
        
        #region Unity Events

        private void Awake()
        {
            _worldVelocity = channel.value * spawnInheritVelocityFactor;
        }

        private void LateUpdate()
        {
            transform.GetPositionAndRotation(out var position, out var rotation);
            position = (float3) position + (_worldVelocity + _velocity) * velocityScale * Time.deltaTime;
            rotation *= math.slerp(quaternion.identity, _angular, Time.deltaTime);

            transform.SetPositionAndRotation(position, rotation);
        }

        #endregion
    }
}
