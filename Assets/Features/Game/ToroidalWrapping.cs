//SPDX-License-Identifier: Unlicense

using Features.Space;
using Unity.Mathematics;
using UnityEngine;
using Tiger.Util;

namespace Features.Game
{
    using Log = Loggers.Create<ToroidalWrapping>;

    [RequireComponent(typeof(VelocityTransformIntegrator))]
    public class ToroidalWrapping : MonoBehaviour
    {
        private WorldBounds _worldBounds;
        private Bounds _wrapBounds;

        private VelocityTransformIntegrator _integrator;
        
        #region Event Functions
        
        protected void Awake()
        {
            _integrator = GetComponent<VelocityTransformIntegrator>();
            SetUpBounds();
        }

        private void LateUpdate()
        {
            Wrap();   
        }

        private void OnTransformChildrenChanged() => SetUpBounds();
        
        #endregion

        #region Editor Events 

        protected void OnValidate()
        {
            if (gameObject.IsAsset()) return;
            if (!GetComponentInParent<WorldBounds>()) Log.Error("No WorldBounds found in parent hierarchy!", this);
        }

        #endregion
        
        private void Wrap()
        {
            float3 position = transform.position;

            if (MovingAway() && OutOfBounds()) 
            {
                transform.position = _wrapBounds.center - (Vector3) position;
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

        private bool OutOfBounds() => !_wrapBounds.Contains(transform.position);

        private bool MovingAway() => math.any(_integrator.velocity * transform.position > 0);
    }
}
