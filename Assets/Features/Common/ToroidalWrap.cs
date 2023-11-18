using UnityEngine;
using Unity.Mathematics;

namespace Features.Common
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ToroidalWrap : MonoBehaviour
    {
        [SerializeField] private Bounds _worldBounds;
        
        private Bounds _wrapBounds;
        private MeshRenderer _ownRenderer;
        
        private float3 _lastPosition;

        private void Awake()
        {
            _lastPosition = transform.position;
            
            _ownRenderer= GetComponent<MeshRenderer>();
            
            _wrapBounds = _worldBounds;
            _wrapBounds.Expand(_ownRenderer.bounds.size);
        }

        public Vector3 speed;

        private void LateUpdate()
        {
            //TODO: Put into motion class.
            transform.position += speed * Time.deltaTime * 3;

            //Derive velocity from position delta
            float3 position = transform.position;
            var velocity = position - _lastPosition;

            if (MovingAway(velocity, position) && OutOfBounds(position))
            {
                transform.position = -position;
                _lastPosition = -position;        
            }
        }

        private bool OutOfBounds(float3 position) => !_wrapBounds.Contains(position);
        
        private bool MovingAway(float3 velocity, float3 position) => math.any(velocity * position > 0);
    }
}
