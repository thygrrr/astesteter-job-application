using UnityEngine;

namespace Features.Motion
{
    [RequireComponent(typeof(IntegratePositionAndRotation))]
    [DisallowMultipleComponent]
    public abstract class ProvideLinearVelocity : MonoBehaviour
    {
        [field: SerializeField]
        protected IntegratePositionAndRotation integrator { get; private set; }

        /// <summary>
        /// Trying this pattern now, to avoid having to use the sealable pattern which is very restrictive.
        /// </summary>
        private void OnValidate()
        {
            integrator = GetComponent<IntegratePositionAndRotation>();            
        }
    }
}