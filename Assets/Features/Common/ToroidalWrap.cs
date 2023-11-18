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

        [SerializeField]
        private Bounds _worldBounds;

        private Bounds ownBounds => _ownRenderer.bounds;
        private Bounds refBounds => _refRenderer.bounds;
        
        private Vector3 _lastPosition;
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
        }

        public Vector3 speed;
        private void LateUpdate()
        {
            //TODO: Put into motion class.
            transform.position += speed * 0.016f * 3;

            //Derive our velocity from our transform change.
            _velocity = transform.position - _lastPosition;
            _lastPosition = transform.position;
            
            // Nothing to do if original is fully inside the bounds.
            KeepOrDespawnReflection();
            WrapOrSwap();
            
            //If we have one reflection, we keep it till it's fully inside the first time.
            if (_reflection.activeSelf)
            {
                //Move reflection with the original object
                _reflection.transform.Translate(_velocity, Space.World);
                _reflection.transform.rotation = transform.rotation;
            } 
        }

        private void KeepOrDespawnReflection()
        {
            if (!_reflection.activeSelf) return;
            
            var fullyInside = _worldBounds.Contains(ownBounds.min) && _worldBounds.Contains(ownBounds.max);
            if (fullyInside)
            {
                _reflection.SetActive(false);
            }

            var fullyOutside = !_worldBounds.Contains(refBounds.min) && !_worldBounds.Contains(refBounds.max);
            if (fullyOutside)
            {
                _reflection.SetActive(false);
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

            //Also wrap position for delta motion, so we don't get a huge velocity spike
            _lastPosition = reflected - _velocity;
        }

        /// <summary>
        /// Wrap transform around the world, and swap with reflection.
        /// </summary>
        private void WrapOrSwap()
        {
            float3 position = transform.position;
            float3 extents = _worldBounds.extents - ownBounds.extents;

            //No action needed if we're moving towards the origin, or are fully inside
            if (math.dot(_velocity, position) < 0 || !math.any(math.abs(position) > extents)) return;

            //We're out AND are moving further out, reflect position
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

        private float3 Reflect(float3 position)
        {
            var absPosition = math.abs(position);

            //pseudo-toroidal wrapping, reflecting around superior axis
            //and slightly twiddling inferior axes for reflection (any axis smaller than the largest)
            var toroidal = -position * math.select(1f, UnityEngine.Random.Range(0.4f, 0.6f), absPosition < math.cmax(absPosition));
            
            //offset by the AABB of the object, so it begins coming in just as the original begins to leave
            toroidal += math.sign(toroidal) * ownBounds.size;
            
            //on some trajectories, the reflection will be fully outside the volume, but
            //the original will be NOT be fully inside. In this case, we need to ensure our
            //new reflection spawns in at the edge of the screen
            float3 wrapSize = _worldBounds.extents - ownBounds.extents;
            toroidal = math.select(toroidal, math.sign(toroidal) * wrapSize, absPosition < math.cmax(absPosition));

            return toroidal;
        }

        
        private void OnDestroy()
        {
            Destroy(_reflection);
        }
    }
}
