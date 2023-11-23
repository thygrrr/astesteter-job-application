//SPDX-License-Identifier: Unlicense

using Tiger.Events;
using Tiger.Events.Concrete;
using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Game
{
    using Log = Loggers.Create<VelocityTransformIntegrator>;

    public class VelocityTransformIntegrator : DataChannelResponder<Vector3Channel, Vector3>
    {
        [SerializeField] private float minOwnVelocity;

        [SerializeField] private float maxOwnVelocity;

        [SerializeField] private float velocityScale = 1;
        [SerializeField] private float spawnInheritVelocityFactor = 1;

        private float3 _ownVelocity;
        private float3 _worldVelocity;

        public float3 velocity => _ownVelocity + _worldVelocity;

        //TODO: Refactor into its own component. 
        public float3 ownVelocity
        {
            get => _ownVelocity;
            set => _ownVelocity = value;
        }

        [SerializeField] private DataChannel<Vector3> lookPosition;

        // NB, with the new coordinate system, this is always at origin.
        // otherwise, we can read the player position from the appropriate channel
        // (using DataChannel<T>.value, not a subscription).
        private static float3 playerPlanar =>0;

        private void Start()
        {
            _worldVelocity = channel.value * spawnInheritVelocityFactor;

            //TODO: This can go into a "InitialOwnVelocity" component instead.
            if (maxOwnVelocity != 0 || minOwnVelocity != 0)
            {
                //"Best" effort to make motion of newly spawned object perpendicular to player position.
                var planar = transform.localPosition._x0z();
                var ccw = Vector3.Cross(planar, lookPosition.value._x0z()).normalized;
                var direction = Vector3.Cross(math.normalizesafe(planar - playerPlanar), ccw);
                var magnitude = minOwnVelocity + maxOwnVelocity * Random.Range(0, 1);
                _ownVelocity = direction * magnitude;

                if (math.abs(_ownVelocity.y) > float.Epsilon)
                    Log.Warn($"Initial Velocity has vertical component, {_ownVelocity.y}", this);
            }
        }

        private void LateUpdate()
        {
            //We're moving in world space, in the XZ plane. (equivalently, we could drive transform.localPosition)
            transform.Translate((_worldVelocity + _ownVelocity) * velocityScale * Time.deltaTime, UnityEngine.Space.World);
        }

        protected override void OnEvent(Vector3 data) => _worldVelocity = data;
    }
}
