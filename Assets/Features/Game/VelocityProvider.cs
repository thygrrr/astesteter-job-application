using Tiger.Events;
using UnityEngine;

namespace Features.Game
{
    [RequireComponent(typeof(VelocityTransformIntegrator))]
    [DisallowMultipleComponent]
    public abstract  class VelocityProvider : MonoBehaviour
    {
        [field: SerializeField]
        protected VelocityTransformIntegrator integrator { get; private set; }

        /// <summary>
        /// Trying this pattern now, to avoid having to use the sealable pattern which is very restrictive.
        /// </summary>
        private void OnValidate()
        {
            integrator = GetComponent<VelocityTransformIntegrator>();            
        }
    }
}