//SPDX-License-Identifier: Unlicense

using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Game
{
    using Log = Loggers.Create<VelocityTransformIntegrator>;
    public class VelocityTransformIntegrator : DataChannelResponder<Vector3Channel, Vector3>
    {
        [SerializeField] 
        private float minOwnVelocity;
        
        [SerializeField] 
        private float maxOwnVelocity;

        private float3 _ownVelocity;
        private float3 _worldVelocity;
        public float3 velocity => _ownVelocity + _worldVelocity;

        [SerializeField] private DataChannel<Vector3> lookPosition;

        // NB, with the new coordinate system, this is always at origin.
        // otherwise, we can read the player position from the appropriate channel
        // (using DataChannel<T>.value, not a subscription).
        private Vector3 playerPosition => Vector3.zero;

        
        private void Start()
        {
            if (maxOwnVelocity == 0 && minOwnVelocity == 0) return;
            
            //TODO: This can go into a spawner component instead.
            
            //"Best" effort to make motion of newly spawned object perpendicular to player position.
            var ccw = Vector3.Cross(playerPosition - transform.position, playerPosition-lookPosition.value).normalized;
            var direction = Vector3.Cross(Vector3.Normalize(transform.position - playerPosition), ccw);
            var magnitude = minOwnVelocity + maxOwnVelocity * Random.Range(0, 1);
            _ownVelocity = direction * magnitude;
        }

        private void LateUpdate()
        {
            transform.Translate((_worldVelocity + _ownVelocity) * Time.deltaTime, UnityEngine.Space.World);
        }

        protected override void OnEvent(Vector3 data) => _worldVelocity = data;
    }
}
