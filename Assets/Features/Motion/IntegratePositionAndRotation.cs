//SPDX-License-Identifier: Unlicense

using System;
using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    using Log = Loggers.Create<IntegratePositionAndRotation>;

    [SelectionBase]
    public class IntegratePositionAndRotation : ProvideIntegration
    {
        [Header("World-Relative Motion")] [SerializeField]
        private float velocityScale = 1;

        [SerializeField] private IntegrationMode mode;

        private Rigidbody _body;


        private enum IntegrationMode
        {
            Static,
            Dynamic,
            Kinematic
        }

        protected override void OnEvent(Vector3 data) => _worldVelocity = data;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _worldVelocity = channel.value;
        }

        private void LateUpdate()
        {
            switch (mode)
            {
                case IntegrationMode.Static:
                    IntegrateStatic();
                    break;
                case IntegrationMode.Dynamic:
                case IntegrationMode.Kinematic:
                    IntegrateRigidBody();
                    break;
            }
        }

        private void IntegrateStatic()
        {
            transform.GetPositionAndRotation(out var position, out var rotation);
            position = (float3) position + (_worldVelocity + _velocity) * velocityScale * Time.deltaTime;
            rotation *= math.slerp(quaternion.identity, _angular, Time.deltaTime);

            transform.SetPositionAndRotation(position, rotation);
        }

        private void IntegrateRigidBody()
        {
            var position = _body.position;
            var rotation = _body.rotation;

            position = (float3) position + (_worldVelocity + _velocity) * velocityScale * Time.deltaTime;
            rotation *= math.slerp(quaternion.identity, _angular, Time.deltaTime);

            _body.MovePosition(position);
            _body.MoveRotation(rotation);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            switch (mode, TryGetComponent(out _body))
            {
                case (IntegrationMode.Static, true):
                    Debug.LogError("Static objects cannot have a rigidbody.", this);                    
                    break;
                
                case (IntegrationMode.Kinematic, false):
                case (IntegrationMode.Dynamic, false):
                    Debug.LogError("Dynamic and Kinematic objects must have a rigidbody.", this);
                    break;
                
                case (IntegrationMode.Dynamic, true):
                    _body.isKinematic = false;
                    break;
                
                case (IntegrationMode.Kinematic, true):
                    _body.isKinematic = true;
                    break;
            }
        }
    }
}
