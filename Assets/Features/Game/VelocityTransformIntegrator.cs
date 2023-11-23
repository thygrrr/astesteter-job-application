//SPDX-License-Identifier: Unlicense

using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Game
{
    using Log = Loggers.Create<VelocityTransformIntegrator>;

    public class VelocityTransformIntegrator : DataChannelResponder<Vector3Channel, Vector3>
    {
        [Header("World-Relative Motion")]
        [SerializeField] private float velocityScale = 1;
        [SerializeField] private float spawnInheritVelocityFactor = 1;

        private float3 _ownVelocity;
        private float3 _worldVelocity;

        public float3 velocity => _ownVelocity + _worldVelocity;

        public float3 ownVelocity
        {
            get => _ownVelocity;
            set
            {
                if (math.abs(value.y) > float.Epsilon) Log.Warn($"Velocity has vertical component, {value}", this);
                _ownVelocity = value;
                _ownVelocity.y = 0;
            } 
        }

        private void Start()
        {
            _worldVelocity = channel.value * spawnInheritVelocityFactor;
        }

        private void LateUpdate()
        {
            //We're moving in world space, in the XZ plane. (equivalently, we could drive transform.localPosition)
            transform.Translate((_worldVelocity + _ownVelocity) * velocityScale * Time.deltaTime, UnityEngine.Space.World);
        }

        protected override void OnEvent(Vector3 data) => _worldVelocity = data;

    }
}
