using Features.Space;
using UnityEngine;
using Unity.Mathematics;

namespace Features.Common
{
    [RequireComponent(typeof(Rigidbody))]
    public class ToroidalWrap : MonoBehaviour
    {
        private World _world;
        private Rigidbody _body;
        private Bounds _wrapBounds;

        #region Event Functions

        private void Awake() => SetUpBounds();

        private void FixedUpdate() => Wrap();

        private void OnTransformChildrenChanged() => SetUpBounds();
        
        #endregion

        #region Editor Events

        private void OnValidate()
        {
            _body = GetComponent<Rigidbody>();
        }

        #endregion

        private void Wrap()
        {
            float3 position = _body.position;
            float3 velocity = _body.velocity;

            if (MovingAway(velocity, position) && OutOfBounds(position))
            {
                _body.position = -position;
            }
        }
        
        private void SetUpBounds()
        {
            _world = FindAnyObjectByType<World>();
            
            var renderBounds = new Bounds();
            foreach (var r in GetComponentsInChildren<Renderer>())
            {
                renderBounds.Encapsulate(r.bounds);
            }

            _wrapBounds = _world.bounds;
            _wrapBounds.Expand(renderBounds.size);
        }

        private bool OutOfBounds(float3 position) => !_wrapBounds.Contains(position);

        private bool MovingAway(float3 velocity, float3 position) => math.any(velocity * position > 0);
    }
}
