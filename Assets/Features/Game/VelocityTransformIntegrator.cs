using Channels.Concrete;
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
        [SerializeField] 
        private float minOwnVelocity;
        [SerializeField] 
        private float maxOwnVelocity;

        private float3 _ownVelocity;
        private float3 _worldVelocity;
        public float3 velocity => _ownVelocity + _worldVelocity;

        private void Start()
        {
            var randomXZ = math.remap(0, 1, -1, 1, new float2(Random.value, Random.value));
            randomXZ = math.normalizesafe(randomXZ);
            _ownVelocity = new float3(randomXZ.x, 0, randomXZ.y) * Random.Range(minOwnVelocity, maxOwnVelocity);
        }

        private void LateUpdate()
        {
            transform.Translate((_worldVelocity + _ownVelocity) * Time.deltaTime, UnityEngine.Space.World);
        }

        protected override void OnEvent(Vector3 data) => _worldVelocity = data;
    }
}
