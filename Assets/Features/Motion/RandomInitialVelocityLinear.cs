using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Motion
{
    public class RandomInitialVelocityLinear : ProvideVelocityLinear
    {
        [SerializeField] [Tooltip("The velocity range of the Object.")]
        private float2 minMaxSpeed = new(30, 50);

        private void Start()
        {
            var magnitude = math.remap(0, 1, minMaxSpeed.x, minMaxSpeed.y, Random.value);
            var rotation = quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
            integrator.velocity += math.mul(rotation, math.forward()) * magnitude;
        }
    }
}
