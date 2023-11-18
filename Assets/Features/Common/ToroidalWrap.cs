using Tiger.Math;
using UnityEngine;
using Unity.Mathematics;

namespace Features.Common
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ToroidalWrap : MonoBehaviour
    {
        private GameObject _reflection;

        private MeshRenderer _ownRenderer;
        private MeshRenderer _refRenderer;

        [SerializeField] private Bounds _worldBounds;

        private bool reflecting
        {
            get => _reflection.activeSelf;
            set => _reflection.SetActive(value);
        }
        private bool originalOutside => !_spawnBounds.Contains(ownBounds.min) && !_spawnBounds.Contains(ownBounds.max);
        private bool reflectionOutside => !_spawnBounds.Contains(refBounds.min) && !_spawnBounds.Contains(refBounds.max);

        private Bounds _spawnBounds;
        private Bounds ownBounds => _ownRenderer.bounds;
        private Bounds refBounds => _refRenderer.bounds;

        private float3 _lastPosition;
        private float3 _velocity;

        private void Awake()
        {
            _lastPosition = transform.position;

            _ownRenderer = GetComponent<MeshRenderer>();

            _reflection = new GameObject("reflection", typeof(MeshRenderer), typeof(MeshFilter), typeof(RenderBounds));
            _reflection.SetActive(false);
            _reflection.transform.localScale = transform.localScale;
            _reflection.GetComponent<MeshFilter>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
            _refRenderer = _reflection.GetComponent<MeshRenderer>();
            _refRenderer.sharedMaterial = _ownRenderer.sharedMaterial;

            _spawnBounds = _worldBounds;
            _spawnBounds.Expand(ownBounds.size);
        }

        public Vector3 speed;

        private void LateUpdate()
        {
            //TODO: Put into motion class.
            transform.position += speed * Time.deltaTime * 3;

            //Derive our velocity from our transform change.
            _velocity = (float3) transform.position - _lastPosition;
            _lastPosition = transform.position;

            UpdateReflection();
            WrapOrSwap();
        }

        private void UpdateReflection()
        {
            if (!reflecting) return;

            //Move reflection with the original object
            _reflection.transform.Translate(_velocity, Space.World);
            _reflection.transform.rotation = transform.rotation;

            //Can we despawn the reflection? (also if we need a new one)
            reflecting = !reflectionOutside;
        }

        /// <summary>
        /// Spawn a ghost / reflection at the exact position of the object.
        /// Usually used right before wrapping away.
        /// </summary>
        private void SpawnReflection()
        {
            var position = transform.position;

            //Wrap original object around in the world to its new position
            var reflected = Reflect(position);
            transform.position = reflected;
            _reflection.transform.position = position;
            reflecting = true;
            
            //Also wrap position for delta motion, so we don't get a huge velocity spike
            _lastPosition = reflected;
        }

        /// <summary>
        /// Wrap transform around the world, and swap with reflection.
        /// </summary>
        private void WrapOrSwap()
        {
            float3 position = transform.position;
            float3 extents = _worldBounds.extents - ownBounds.extents;

            //No action needed if we are fully inside the world bounds
            if (!math.any(math.abs(position) > extents)) return;
            
            // We totally should have a reflection!
            if (!reflecting) SpawnReflection();
            
            //when original is fully out of the world, we swap with our reflection
            if (originalOutside)
            {
                // we already have a reflection, so we just swap
                transform.position = _reflection.transform.position;
                // fix up velocity after swap
                _lastPosition = transform.position;

                reflecting = false;
            }
        } 

        /// <summary>
        /// Pseudo-toroidal wrapping, reflecting a position around any axis that is individually moving away from the origin
        /// </summary>
        /// <param name="position"></param>
        /// <returns>true if calculated toroidal reflection is a spawnable location</returns>
        private float3 Reflect(float3 position)
        {
            return 0.99f * _spawnBounds.ClosestPoint(position * -2);
        }

        
        private void OnDestroy()
        {
            Destroy(_reflection);
        }
    }
}
