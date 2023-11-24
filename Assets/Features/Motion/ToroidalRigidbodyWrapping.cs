//SPDX-License-Identifier: Unlicense

using Features.Space;
using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    using Log = Loggers.Create<ToroidalWrapping>;
    
    [RequireComponent(typeof(Rigidbody))]
    public class ToroidalRigidBodyWrapping : MonoBehaviour
    {
        [Header("Virtual sizing for small objects")]
        [Tooltip("This is used so small objects like bullets don't wrap too early, and so they can hit large, currently wrapping objects.")]
        [SerializeField] private float wrapPadding = 10f;
        
        private WorldBounds _world;
        private Rigidbody _body;
        private Bounds _wrapBounds;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            SetUpBounds();   
        }

        private void FixedUpdate() => Wrap(); //sic! We're using physics with dynamic update

        private void Wrap()
        {
            float3 position = _body.position;
            float3 velocity = _body.velocity;

            var planar = _body.position.fx0z();
            var origin = _world.bounds.center.fx0z();
            var wrapBounds = _world.bounds;
            wrapBounds.Expand(wrapPadding);

            var outOfBounds = !wrapBounds.Contains(planar);
            var movingAway = math.any(velocity * (planar - origin) > 0);


            if (outOfBounds && movingAway)
            {
                //Only wrap the largest coordinate.
                var wrapped = math.select(origin - planar, planar, math.abs(planar) < math.cmax(math.abs(planar)));
                wrapped.y = -transform.localPosition.y; //allows us to have non-gameplay objects not all be in one plane
                _body.position = wrapped;
            }
        }
        
        private void SetUpBounds()
        {
            _world = GetComponentInParent<WorldBounds>();
            if (!_world) Log.Error("No world bounds found in parent!", this);
            
            _wrapBounds = _world.bounds; 
            //We're not using the render bounds because this component is for small, rigidbody objects.
            _wrapBounds.Expand(wrapPadding);
        }

        private bool OutOfBounds(float3 position) => !_wrapBounds.Contains(position);

        private bool MovingAway(float3 velocity, float3 position) => math.any(velocity * position > 0);
    }
}
