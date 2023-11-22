using UnityEngine;

namespace Features.Space
{
    [ExecuteAlways]
    public class WorldBounds : MonoBehaviour
    {
        public Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 100);
        
        private static readonly int toroidalCameraExtents = Shader.PropertyToID("_ToroidalCameraExtents");
        
        private void Update()
        {
            Shader.SetGlobalVector(toroidalCameraExtents, bounds.extents);
        }

        #region Editor Events

        private void OnValidate()
        {
            transform.position = default;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        #endregion
    }
}
