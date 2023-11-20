using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Space
{
    public class WorldBounds : MonoBehaviour
    {
        [SerializeField] private Vector2 size;
        public Bounds bounds => new(default, size._x0y());
        
        private static readonly int toroidalCameraExtents = Shader.PropertyToID("_ToroidalCameraExtents");
        
        #region Editor Events
        private void Update()
        {
            Shader.SetGlobalVector(toroidalCameraExtents, bounds.extents);
        }

        private void OnValidate()
        {
            size = math.max(size, 0);
            if (size == default) size = new Vector2(20,  20);
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
