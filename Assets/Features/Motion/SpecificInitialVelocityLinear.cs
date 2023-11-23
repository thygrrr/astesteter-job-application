using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Motion
{
    public class SpecificInitialVelocityLinear : ProvideVelocityLinear
    {
        [SerializeField] [Tooltip("The velocity the Object (local space).")]
        private float3 initialVelocity;

        private void Start()
        {
            integrator.velocity = transform.TransformDirection(initialVelocity);
        }
    }
}
