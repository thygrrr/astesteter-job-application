using UnityEngine;
using Unity.Mathematics;
namespace Features.Common
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ToroidalReflectionWrap : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private GameObject _reflection;

        [SerializeField]
        private Bounds _worldBounds;
        private Bounds bounds => _meshRenderer.bounds;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            
            _reflection = new GameObject("reflection", typeof(MeshRenderer), typeof(MeshFilter));
            _reflection.transform.localScale = transform.localScale;
            _reflection.GetComponent<MeshFilter>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
            _reflection.GetComponent<MeshRenderer>().sharedMaterial = _meshRenderer.sharedMaterial;
        }

        private void LateUpdate()
        {
            var fullyInside = _worldBounds.Contains(bounds.min) && _worldBounds.Contains(bounds.max);
            if (fullyInside)
            {
                _reflection.SetActive(false);
                return;
            }

            float3 position = transform.position;
            float3 extents = _worldBounds.extents;
            float3 maxExtents = _worldBounds.extents - bounds.extents;
            float3 minExtents = -_worldBounds.extents + bounds.extents;
            float3 mirrorSize = _worldBounds.size - bounds.size;
            float3 wrapSize = _worldBounds.size;

            var mirrored = math.select(position - mirrorSize, position + mirrorSize, position < 0);
            mirrored.y = 0;
            
            var greaterWrapped = math.select(position, position - wrapSize, position > maxExtents);
            var fullyWrapped = math.select(greaterWrapped, position + wrapSize, position < minExtents);
            fullyWrapped.y = 0;
            
            //var fullyWrapped = math.select(position, -position, math.abs(position) > maxExtents);
            
            _reflection.transform.position = fullyWrapped;
            _reflection.SetActive(true); 
        }

        private void OnDestroy()
        {
            Destroy(_reflection);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var bounds = GetComponent<MeshRenderer>().bounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
