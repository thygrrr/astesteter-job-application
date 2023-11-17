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
            float3 wrapSize = _worldBounds.size;

            var diagonalOffset = position + wrapSize*0.5f;
            diagonalOffset.y = 0;

            var wrapped = diagonalOffset;
            while (math.any(math.abs(wrapped) > wrapSize))
            {
                var greaterWrapped = math.select(wrapped, wrapped - wrapSize, wrapped > wrapSize);
                var fullyWrapped = math.select(greaterWrapped, wrapped + wrapSize, wrapped < -wrapSize);
                fullyWrapped.y = 0;
                wrapped = fullyWrapped;
            }
            
            _reflection.transform.position = wrapped;
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
