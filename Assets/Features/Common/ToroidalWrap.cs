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
        }

        public Vector3 speed;
        private void LateUpdate()
        {
            //TODO: Put into motion class.
            transform.position += speed * Time.deltaTime * 3;

            //Derive our velocity from our transform change.
            _velocity = (float3) transform.position - _lastPosition;
            _lastPosition = transform.position;
            
            //If we have one reflection, we keep it till it's fully inside the first time.
            if (_reflection.activeSelf)
            {
                //Move reflection with the original object
                _reflection.transform.Translate(_velocity, Space.World);
                _reflection.transform.rotation = transform.rotation;
            }

            WrapOrSwap();
            KeepOrDespawnReflection();
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

            //Stretch largest component of reflected position to be guaranteed outside the world bounds
            //Reason: on some trajectories, the reflection will be fully outside the volume (so we need a new one),
            //but the original will be NOT YET be fully inside (usually skirting across edges/corners).
            //In this case, we want to ensure our new reflection will not spawn already overlapping the world bounds.
            float3 wrapSize = _worldBounds.extents + ownBounds.extents;
            //var absToroidal = math.abs(reflected);
            //var stretched = math.select(reflected, math.sign(reflected) * wrapSize, absToroidal < math.cmax(absToroidal));
            //reflected.xz = stretched.xz;
            math.clamp(reflected, -wrapSize, wrapSize);
            
            transform.position = reflected;

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

            //No action needed if we're moving towards the origin, or are fully inside
            if (math.all(math.abs(position.xz) < extents.xz)) return;
            //if (math.dot(_velocity, position) < 0) return;
            
            //We're out AND are further away than our reflection, swap position
            if (_reflection.activeSelf)
            {
                // we already have a reflection, so we just swap
                (_reflection.transform.position, transform.position) = (transform.position, _reflection.transform.position);

                // fix up velocity after swap
                _lastPosition = (float3) transform.position;
            }
            else
            {
                SpawnReflection();
            }
        }

        /// <summary>
        /// Pseudo-toroidal wrapping, reflecting a position around any axis that is individually moving away from the origin
        /// </summary>
        /// <param name="position"></param>
        /// <returns>the wrapped equivalent position</returns>
        private float3 Reflect(float3 position)
        {
            float3 ownSize =ownBounds.size;
            float3 wrapSize = _worldBounds.extents + ownBounds.extents;

            //offset by the AABB of the object, so it begins coming in just right👌 as the original begins to leave
            var aabbOffset = math.sign(position) * ownSize;
            var toroidal = math.select(position, -(position + aabbOffset), _velocity * position >= 0);
            
            return toroidal;
        }

        
        private void OnDestroy()
        
        {
            Destroy(_reflection);
        }
    }
}
