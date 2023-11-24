//SPDX-License-Identifier: Unlicense

using Channels.Concrete;
using Tiger.Events;
using Tiger.Events.Concrete;
using Tiger.Math;
using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Space
{
    using Log = Loggers.Create<VelocityDrivenOrbit>;

    public class VelocityDrivenOrbit : DataChannelResponder<Vector3Channel, Vector3>
    {
        [SerializeField] [Tooltip("scale velocity of body by this amount")]
        private float bodyVelocityScale = 1;

        [Header("These ain't your normal Kepler-ian Elements")][SerializeField] [Tooltip("Length of one circumnavigation in unity Units")]
        private float period = 1000;

        [SerializeField] [Tooltip("Lowest point of orbit")]    
        private float periapsis = 300;
        
        [SerializeField] [Tooltip("Highest point of orbit")]
        private float apoapsis = 900;

        [Header("Driving Behaviour")]
        [SerializeField] [Tooltip("Object to move and rotate underneath the body.")]
        private Transform barycenter;

        // The orbit tracks its own toroidal position, disregarding what other objects may use for their wrapping.
        private float2 _toroidalPosition;
        private Vector3 _bodyVelocity;

        private void LateUpdate()
        {
            var dt = Time.deltaTime;
            var velocity = _bodyVelocity.fxz() * bodyVelocityScale;
            _toroidalPosition += velocity * dt;
            _toroidalPosition = mathex.eumod(_toroidalPosition, period);
        
            var location = math.smoothstep(0, period * math.SQRT2 * 0.5f, math.length(_toroidalPosition));
            var altitude = math.lerp(apoapsis, periapsis, location);
            var position = Vector3.down * altitude;

            var orbitalDelta = velocity * bodyVelocityScale * dt * location * 0.25f; //fudged, quarter-angle feels about right
            var angularDelta = Quaternion.Euler(orbitalDelta.y, 0, -orbitalDelta.x);
            var rotation= angularDelta * barycenter.localRotation;
        
            barycenter.SetLocalPositionAndRotation(position, rotation);
        }

        protected override void OnEvent(Vector3 velocity)
        {
            _bodyVelocity = velocity;
        }

        protected override void OnValidate()
        {
            period = math.max(period, 1);
            apoapsis = math.abs(apoapsis);
            periapsis = math.abs(periapsis);

            if (barycenter)
            {
                barycenter.localPosition = Vector3.down * apoapsis;
            }
            else
            {
                Log.Warn("No barycenter (backdrop object) set", this);
            }
        }
    }
}
