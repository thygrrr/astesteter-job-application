using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Space
{
    using Log = Loggers.Create<SunOcclusion>;

    [RequireComponent(typeof(Light))]
    public class SunOcclusion : DataChannelEmitter<BoolChannel, bool>
    {
        [SerializeField] private LayerMask occlusionLayers;
    
        private Camera _camera;
        private State _occlusion;

        private enum State
        {
            Unknown = default,
            Occluded,
            Visible
        }
    
        private State occlusionState
        {
            set
            {
                if (_occlusion == value || value == State.Unknown) return;
                _occlusion = value;
                Emit(value == State.Visible);
            }
        }

        #region Unity Events
        private void Awake() => _camera = Camera.main;

        private void Start() => CheckOcclusion();

        private void FixedUpdate() => CheckOcclusion();
    
        private void CheckOcclusion()
        {
            // cast from camera as our light is likely directional and doesn't have a real "position"
            // (this covers both LightType.Directional and LightType.Point)
            var origin = _camera.transform.position;
            var direction = -transform.forward;
            var distance = Vector3.Distance(origin, transform.position);
            var ray = new Ray(origin, direction);
        
            occlusionState = Physics.Raycast(ray, distance, occlusionLayers) ? State.Occluded : State.Visible;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (occlusionLayers == default) occlusionLayers = LayerMask.GetMask("Backdrop");
            var type = GetComponent<Light>().type;
            if (type != LightType.Directional && type != LightType.Point) Log.Error("Only LightType.Directional or LightType.Point are supported.", this);
        }
    
        #endregion
    }
}
