using System.Collections;
using Features.Game;
using Features.Motion;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Enemies
{
    [RequireComponent(typeof(IntegratePositionAndRotation))]
    public class RandomZigZagCourse : ProvideVelocityLinear
    {
        [SerializeField] [Tooltip("The velocity range of the Object.")]
        private float2 minMaxSpeed = new(30, 50);
        [SerializeField] [Tooltip("Half-life of the smoothing frequency.")] 
        private float zigZagLambda = 1;

        private Vector3 _velocityGoal;
        private Vector3 _velocityDerivative;

        private IEnumerator Start()
        {
            while (Application.isPlaying)
            {
                SetRandomVelocityGoal();
                yield return new WaitForSeconds(4);
            }
        }

        private void Update()
        {
            integrator.velocity = Vector3.SmoothDamp(integrator.velocity, _velocityGoal, ref _velocityDerivative, zigZagLambda);
        }

        private void SetRandomVelocityGoal()
        {
            var magnitude = math.remap(0, 1, minMaxSpeed.x, minMaxSpeed.y, Random.value);
            var rotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
            _velocityGoal = rotation * Vector3.forward * magnitude;
        }
    }
}
