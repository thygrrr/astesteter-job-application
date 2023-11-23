using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Space
{
    [ExecuteAlways]
    public class WorldBounds : MonoBehaviour
    {
        [Header("Game")] 
        [SerializeField] private bool driveGlobalShader = true;
        [SerializeField] private Vector3 size = Vector3.one * 100;

        [Header("Editor")] 
        [SerializeField] private Color gizmoColor = Color.yellow;
        
        public Bounds bounds => new(transform.position, size);
        
        private static readonly int toroidalCameraExtents = Shader.PropertyToID("_ToroidalCameraExtents");
        
        private void Update()
        {
            if (driveGlobalShader) Shader.SetGlobalVector(toroidalCameraExtents, bounds.extents);
        }

        #region Editor Events

        private void OnValidate()
        {
            transform.position = default;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        #endregion
    }
}
