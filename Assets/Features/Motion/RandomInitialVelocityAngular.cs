using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Motion
{
    [RequireComponent(typeof(IntegratePositionAndRotation))]
    public class RandomInitialVelocityAngular : ProvideVelocityAngular
    {
        [SerializeField] [Tooltip("The velocity range of the Object, in degrees per second.")]
        private float2 minMaxSpeed = new(0, 0);

        private void Start()
        {
            var radians = math.radians(minMaxSpeed);
            integrator.angular = quaternion.Euler(Random.onUnitSphere * math.lerp(radians.x, radians.y, Random.value));
        }
    }
}