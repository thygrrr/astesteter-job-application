using UnityEngine;
using Unity.Mathematics;

namespace Features.Common
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ToroidalWrap : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private GameObject _reflection;

        [SerializeField]
        private Bounds _worldBounds;
        private Bounds bounds => _meshRenderer.bounds;
        
        private Vector3 _lastPosition;
        private float3 _velocity;
        
        private void Awake()
        {
            _lastPosition = transform.position;
            
            _meshRenderer = GetComponent<MeshRenderer>();
           
            _reflection = new GameObject("reflection", typeof(MeshRenderer), typeof(MeshFilter), typeof(RenderBounds));
            _reflection.SetActive(false);
            _reflection.transform.localScale = transform.localScale;
            _reflection.GetComponent<MeshFilter>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
            _reflection.GetComponent<MeshRenderer>().sharedMaterial = _meshRenderer.sharedMaterial;
        }

        public Vector3 speed;
        private void LateUpdate()
        {
            _velocity = speed * Time.deltaTime * 3;
            transform.position += (Vector3) _velocity;

            WrapOrSwap();
            
            // Nothing to do if original is fully inside the bounds.
            var fullyInside = _worldBounds.Contains(bounds.min) && _worldBounds.Contains(bounds.max);
            if (fullyInside)
            {
                _reflection.SetActive(false);
                return;
            }
            
            //If we have one reflection, we keep it till it's fully inside the first time.
            if (_reflection.activeSelf)
            {
                //Move reflection with the original object
                _velocity = transform.position - _lastPosition;
                _reflection.transform.Translate(_velocity);
                _reflection.transform.rotation = transform.rotation;
                _lastPosition = transform.position;
            } 
        }

        /// <summary>
        /// Spawn a ghost / reflection at the exact position of the object.
        /// Usually used right before wrapping away.
        /// </summary>
        private void SpawnReflection()
        {
            var position = transform.position;
            
            _reflection.transform.position = position;
            _reflection.SetActive(true);

            //Wrap original object around in the world to its new position
            var reflected = Reflect(position);
            transform.position = reflected;

            //Warp position for delta motion
            _lastPosition = reflected;
        }

        /// <summary>
        /// Wrap transform around the world, and swap with reflection.
        /// </summary>
        private void WrapOrSwap()
        {
            float3 position = transform.position;
            float3 extents = _worldBounds.extents;

            //If we're out AND are moving further out, reflect position
            if (math.any(math.abs(position) > extents) && math.dot(_velocity, position) > 0)
            {
                if (_reflection.activeSelf)
                {
                    // we already have a reflection, so we just swap
                    (_reflection.transform.position, transform.position) = (transform.position, _reflection.transform.position);
                }
                else
                {
                    SpawnReflection();
                }
            }
        }

        private float3 Reflect(float3 position)
        {
            var wrapSize = (float3) (_worldBounds.extents);
            var absPosition = math.abs(position);

            //pseudo-toroidal wrapping, reflecting around superior axis
            //and slightly twiddling inferior axes for reflection (any axis smaller than the largest)
            var toroidal = position * math.select(-1f, UnityEngine.Random.Range(0.9f, 1.1f), math.cmax(absPosition) > absPosition);
            
            //offset by the AABB of the object, so it begins coming in just as the original begins to leave
            toroidal += math.sign(toroidal) * bounds.size;
            
            return math.clamp(toroidal, -wrapSize + float.Epsilon, wrapSize - float.Epsilon);
        }

        
        private void OnDestroy()
        {
            Destroy(_reflection);
        }
    }
}
