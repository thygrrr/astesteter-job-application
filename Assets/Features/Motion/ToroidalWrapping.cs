//SPDX-License-Identifier: Unlicense

using Tiger.Math;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    [RequireComponent(typeof(IntegratePositionAndRotation))]
    public class ToroidalWrapping : MonoBehaviour
    {
        private WorldBounds _world;

        private Vector3 ownSize
        {
            get
            {
                if (_renderers.Length == 0) return default;
                
                //We need to evaluate this lazily as the engine updates this each frame
                var bounds = _renderers[0].bounds;
                
                // There usually is only one.
                for (var i = 1; i < _renderers.Length; i++)
                {
                    bounds.Encapsulate(_renderers[i].bounds);
                }

                return bounds.size;
            }
        }
        private Renderer[] _renderers;

        private IntegratePositionAndRotation _integrator;
        
        #region Event Functions
        
        protected void Awake()
        {
            _world = GetComponentInParent<WorldBounds>();
            _integrator = GetComponent<IntegratePositionAndRotation>();
            _renderers = GetComponentsInChildren<Renderer>(); 
        }

        private void LateUpdate()
        {
            Wrap();   
        }

        private void OnTransformChildrenChanged() => _renderers = GetComponentsInChildren<Renderer>();
        
        #endregion
        
        private void Wrap()
        {
            var planar = transform.localPosition.fx0z();
            var origin = _world.bounds.center.fx0z();
            var wrapBounds = _world.bounds;
            wrapBounds.Expand(ownSize);

            //Only wrap coordinates that are outside the bounds and whose dimension is moving away
            if (!wrapBounds.Contains(planar))
            {
                var outOfBounds = math.abs(planar - (float3) wrapBounds.center) > wrapBounds.extents;
                var movingAway = _integrator.finalVelocity * (planar - origin) > 0;
                var wrapped = math.select(planar, origin - planar, movingAway & outOfBounds);

                wrapped.y = -transform.localPosition.y; //allows us to have non-gameplay objects not all be in one plane
                transform.localPosition = wrapped;
            }
        }
    }
}
