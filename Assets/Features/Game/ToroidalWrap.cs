//SPDX-License-Identifier: Unlicense
using Features.Space;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Features.Game
{
    using Log = Loggers.Create<ToroidalWrap>;
    
    [RequireComponent(typeof(Rigidbody))]
    public class ToroidalWrap : MonoBehaviour
    {
        private WorldBounds _worldBounds;
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
            var isInstance = PrefabUtility.IsPartOfPrefabInstance(this);
            if (isInstance && !GetComponentInParent<WorldBounds>()) Log.Error("No WorldBounds found in parent hierarchy!", this);
        }

        #endregion

        private void Wrap()
        {
            float3 position = _body.position;
            float3 velocity = _body.velocity;

            if (MovingAway(velocity, position) && OutOfBounds(position))
            {
                _body.position = _wrapBounds.center - (Vector3) position;
            }
        }
        
        private void SetUpBounds()
        {
            _worldBounds = GetComponentInParent<WorldBounds>();
            
            var renderBounds = new Bounds();
            foreach (var r in GetComponentsInChildren<Renderer>())
            {
                renderBounds.Encapsulate(r.bounds);
            }

            _wrapBounds = _worldBounds.bounds;
            _wrapBounds.Expand(renderBounds.size);
        }

        private bool OutOfBounds(float3 position) => !_wrapBounds.Contains(position);

        private bool MovingAway(float3 velocity, float3 position) => math.any(velocity * position > 0);
    }
}
