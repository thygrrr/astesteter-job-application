//SPDX-License-Identifier: Unlicense

using Features.Space;
using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    [RequireComponent(typeof(IntegratePositionAndRotation))]
    public class ToroidalWrapping : MonoBehaviour
    {
        private WorldBounds _world;
        private Vector3 _ownSize;

        private IntegratePositionAndRotation _integrator;
        
        #region Event Functions
        
        protected void Awake()
        {
            _integrator = GetComponent<IntegratePositionAndRotation>();
            DetermineOwnSize();
        }

        private void LateUpdate()
        {
            Wrap();   
        }

        private void OnTransformChildrenChanged() => DetermineOwnSize();
        
        #endregion
        
        private void Wrap()
        {
            var planar = transform.localPosition._x0z();
            var origin = _world.bounds.center._x0z();
            var wrapBounds = _world.bounds;
            wrapBounds.Expand(_ownSize);
            
            var outOfBounds = !wrapBounds.Contains(planar);
            var movingAway = math.any(_integrator.finalVelocity * (planar-origin) > 0);

            if (outOfBounds && movingAway)
            {
                var wrapped = origin - planar;
                wrapped.y = -transform.localPosition.y; //allows us to have non-gameplay objects not all be in one plane
                transform.localPosition = wrapped;
            }
        }
        
        private void DetermineOwnSize()
        {
            _world = GetComponentInParent<WorldBounds>();            
            
            var renderBounds = new Bounds();
            foreach (var r in GetComponentsInChildren<Renderer>())
            {
                renderBounds.Encapsulate(r.bounds);
            }

            _ownSize = new float3(math.cmax(renderBounds.size));
        }
    }
}
