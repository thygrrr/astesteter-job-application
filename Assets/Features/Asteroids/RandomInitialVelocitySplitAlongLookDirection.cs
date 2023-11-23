using Features.Game;
using Tiger.Events;
using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Asteroids
{
    using Log = Loggers.Create<RandomInitialVelocitySplitAlongLookDirection>;

    [RequireComponent(typeof(VelocityTransformIntegrator))]
    public class RandomInitialVelocitySplitAlongLookDirection : VelocityProvider
    {
        [SerializeField] [Tooltip("The velocity range of the Object.")]
        private float2 minMaxSpeed = new(0, 0);

        [Header("Player direction.")] [SerializeField]
        private DataChannel<Vector3> lookDirection;

        private void Start()
        {
            var planar = math.normalizesafe(transform.localPosition._x0z());
            var direction = math.normalizesafe(lookDirection.value);
            
            //"Best" effort to make motion of newly spawned object perpendicular to player direction.
            var ccwUpDown = Vector3.Cross(planar, direction);
            var perpendicular = Vector3.Cross(direction, ccwUpDown);

            var magnitude = math.remap(0, 1, minMaxSpeed.x, minMaxSpeed.y, Random.value);
            var velocity = perpendicular * magnitude;

            integrator.ownVelocity = velocity;
        }
    }
}