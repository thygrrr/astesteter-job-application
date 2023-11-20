using Channels.Concrete;
using Feature.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using Tiger.Swizzles;
using Tiger.Math;

namespace Features.Player
{
    public class ShipThrusterControls : MonoBehaviour, GameInputActions.IPlayerActions
    {
        [Tooltip("Channel to send acceleration data through")]
        [SerializeField] private Vector3Channel accelerationChannel;

        [Header("Thrust & Turning")]
        [SerializeField] [Tooltip("Forward Acceleration (units/second²)")]
        private float engineThrust = 20;

        [SerializeField] [Tooltip("Thrust Decay (half life)")]
        private float engineLambda = 0.1f;

        [SerializeField] [Tooltip("Rotation SmoothTime (half life)")]
        private float rotationLambda = 0.1f;

        [SerializeField] 
        private GameObject forwardFx;
        
        private Camera _camera;

        private float _thrust;
        private float _thrustTarget;
        private float _thrustDerivative;

        private Quaternion _rotation;
        private Quaternion _rotationTarget;
        private Quaternion _rotationDerivative;

        public void Awake()
        {
            _camera = Camera.main;
            forwardFx.SetActive(false);
        }

        private void Update()
        {
            //We rotate in dynamic update to make it extra smooth.
            _rotation = QuatEx.SmoothDamp(_rotation, _rotationTarget, ref _rotationDerivative, rotationLambda, Time.deltaTime);
            transform.rotation = _rotation;
        }

        private void FixedUpdate()
        {
            // Decay / Gain thrust
            _thrust = Mathf.SmoothDamp(_thrust, _thrustTarget, ref _thrustDerivative, engineLambda);
            
            var acceleration = _thrust * -transform.forward;
            accelerationChannel.Invoke(acceleration._x0z());
            
            forwardFx.gameObject.SetActive(_thrust > 0.25f);
        }

        public void OnThrust(InputAction.CallbackContext context)
        {
            _thrustTarget = context.action.IsPressed() ? engineThrust : 0;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            var mouse = Mouse.current.position.ReadValue();

            var ray = _camera.ScreenPointToRay(mouse);
            var plane = new Plane(Vector3.up, Vector3.zero);

            if (!plane.Raycast(ray, out var distance)) return;

            var point = ray.GetPoint(distance);
            var direction = point - transform.position;
            
            _rotationTarget = Quaternion.LookRotation(direction.normalized, transform.up);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
        }
    }
}
