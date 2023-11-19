using Tiger.Math;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Space
{
    public class World : MonoBehaviour
    {
        public Bounds bounds => new(default, size._x0y());

        [SerializeField] private Vector2 size;

        #region Editor Events

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
