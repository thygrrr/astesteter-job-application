//SPDX-License-Identifier: Unlicense

using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    using Log = Loggers.Create<IntegrateRigidBodyPositionAndRotation>;

    [RequireComponent(typeof(Rigidbody))]
    public class IntegrateRigidBodyPositionAndRotation : ProvideIntegration
    {
        [Header("World-Relative Motion")] [SerializeField]
        private float velocityScale = 1;

        private Rigidbody _body;
        
        protected override void OnEvent(Vector3 data) => _worldVelocity = data;
        
        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _worldVelocity = channel.value;
        }

        private void FixedUpdate() //sic - we're using physics with dynamic update (see Project Settings -> Physics)
        {
            var position = _body.position;
            var rotation = _body.rotation;

            position = (float3) position + (_worldVelocity + _velocity) * velocityScale * Time.deltaTime;
            rotation *= math.slerp(quaternion.identity, _angular, Time.deltaTime);

            _body.MovePosition(position);
            _body.MoveRotation(rotation);
        }
    }
}
