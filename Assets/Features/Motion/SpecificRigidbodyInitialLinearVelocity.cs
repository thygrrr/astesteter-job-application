using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Motion
{
    [RequireComponent(typeof(Rigidbody))]
    public class SpecificRigidBodyInitialLinearVelocity : ProvideLinearVelocity
    {
        [SerializeField] [Tooltip("The velocity the Object (local space).")]
        private float3 initialVelocity;

        private void Start()
        {
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(initialVelocity);
        }
    }
}
