//SPDX-License-Identifier: Unlicense

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

        protected override void OnEvent(Vector3 data) => _worldVelocity = data;
        
        #region Unity Events

        private void Awake()
        {
            _worldVelocity = channel.value;
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
