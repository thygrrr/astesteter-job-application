using Unity.Mathematics;
using UnityEngine;

namespace Features.Motion
{
    [ExecuteAlways]
    public class WorldBounds : MonoBehaviour
    {
        [Header("Game")] 
        [SerializeField] private Vector3 size = Vector3.one * 100;
        [Header("Shader")]
        [SerializeField] private string globalExtentsParameter = "_ToroidalWorldExtents";
        [SerializeField] private string globalOriginParameter = "_ToroidalWorldOrigin";

        [Header("Editor")] 
        [SerializeField] private Color gizmoColor = Color.yellow;
        
        public Bounds bounds => new(transform.position, size);

        private int _shaderExtentsId;
        private int _shaderOriginId;

        private void Awake()
        {
            _shaderExtentsId = Shader.PropertyToID(globalExtentsParameter);
            _shaderOriginId = Shader.PropertyToID(globalOriginParameter);
        }

        private void Update()
        {
            Shader.SetGlobalVector(_shaderExtentsId, size * 0.5f);
            Shader.SetGlobalVector(_shaderOriginId, transform.position);
        }

        #region Editor Events

        private void OnValidate()
        {
            //This will likely still be needed before we can refactor the maths in the integrators.
            transform.rotation = quaternion.identity;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        #endregion
    }
}
