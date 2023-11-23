using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    [RequireComponent(typeof(Rigidbody))]
    public class SpecificRigidBodyInitialVelocityLinear : MonoBehaviour
    {
        [SerializeField] [Tooltip("The velocity the Object (local space).")]
        private float3 initialVelocity;

        private void Start()
        {
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(initialVelocity);
        }
    }
}
