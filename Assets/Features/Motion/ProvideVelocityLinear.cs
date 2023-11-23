using UnityEngine;

namespace Features.Motion
{
    [RequireComponent(typeof(ProvideIntegration))]
    [DisallowMultipleComponent]
    public abstract class ProvideVelocityLinear : MonoBehaviour
    {
        [field: SerializeField]
        protected ProvideIntegration integrator { get; private set; }

        /// <summary>
        /// Trying this pattern now, to avoid having to use the sealable pattern which is very restrictive.
        /// </summary>
        private void OnValidate()
        {
            integrator = GetComponent<ProvideIntegration>();            
        }
    }
}