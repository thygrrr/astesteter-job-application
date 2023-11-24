using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    public class SpecificInitialVelocityLinear : ProvideVelocityLinear
    {
        [SerializeField] [Tooltip("The velocity the Object (local space).")]
        private float3 initialVelocity;

        private void Start()
        {
            //Sometimes gives y values > epsilon
            integrator.velocity = transform.TransformDirection(initialVelocity)._x0z();
        }
    }
}
