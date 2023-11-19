using Tiger.Math;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Space
{
    public class BodyDrivenOrbit : MonoBehaviour
    {
        [Header("Body & Orbital velocities")] [SerializeField] [Tooltip("Orbit auto-advances in this direction")]    
        private float2 intrinsicVelocity = 20;

        [SerializeField] [Tooltip("scale velocity of body by this amount")]
        private float bodyVelocityScale = 5;

        [Header("These ain't your normal Kepler-ian Elements")][SerializeField] [Tooltip("Length of one circumnavigation in unity Units")]
        private float period = 1000;

        [SerializeField] [Tooltip("Lowest point of orbit")]    
        private float periapsis = 300;
        
        [SerializeField] [Tooltip("Highest point of orbit")]
        private float apoapsis = 900;

        [Header("Driving Behaviour")]
        [SerializeField] [Tooltip("Body to follow (e.g. Player)")]
        private Rigidbody body;

        [SerializeField] [Tooltip("Object to move and rotate underneath the body.")]
        private Transform barycenter;

        // The orbit tracks its own toroidal position, disregarding what other objects may use for their wrapping.
        private float2 _toroidalPosition;
        private void LateUpdate()
        {
            var dt = Time.deltaTime;
            var velocity = intrinsicVelocity + body.velocity._xz() * bodyVelocityScale;
            _toroidalPosition += velocity * dt;
            _toroidalPosition = mathex.eumod(_toroidalPosition, period);
        
            var location = math.smoothstep(0, math.SQRT2 * period, math.length(_toroidalPosition));
            var altitude = math.lerp(apoapsis, periapsis, location);
            var position = Vector3.down * altitude;

            var orbitalDelta = velocity / math.sqrt(altitude) * dt;
            var angularDelta = Quaternion.Euler(orbitalDelta.y, 0, -orbitalDelta.x);
            var rotation= angularDelta * barycenter.localRotation;
        
            barycenter.SetLocalPositionAndRotation(position, rotation);
        }

        private void OnValidate()
        {
            period = math.max(period, 1);
            apoapsis = math.abs(apoapsis);
            periapsis = math.clamp(periapsis, 0, apoapsis);

            if (barycenter)
            {
                barycenter.localPosition = Vector3.down * apoapsis;
            }
            else
            {
                Debug.LogWarning("BodyDrivenOrbit: No barycenter (backdrop object) set");
            }
        }
    }
}
