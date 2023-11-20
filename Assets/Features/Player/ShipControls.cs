using Channels.Concrete;
using Feature.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using Tiger.Swizzles;
using Tiger.Math;
using Tiger.ScreenShake;

namespace Features.Player
{
    public class ShipControls : MonoBehaviour, GameInputActions.IPlayerActions
    {
        [Tooltip("Channel to send acceleration data through")] [SerializeField]
        private Vector3Channel accelerationChannel;

        [Header("Thrust & Turning")] [SerializeField] [Tooltip("Forward Acceleration (units/second²)")]
        private float engineThrust = 20;

        [SerializeField] [Tooltip("Thrust Decay (half life)")]
        private float engineLambda = 0.1f;

        [SerializeField] [Tooltip("Rotation SmoothTime (half life)")]
        private float rotationLambda = 0.1f;

        [SerializeField] private GameObject forwardFx;

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
            OrientShipFromMouse();
            IntegrateRotation();
        }

        private void FixedUpdate() => IntegrateAcceleration();

        private void IntegrateRotation()
        {
            //We rotate in dynamic update to make it extra smooth.
            _rotation = QuatEx.SmoothDamp(_rotation, _rotationTarget, ref _rotationDerivative, rotationLambda, Time.deltaTime);
            transform.rotation = _rotation;

            forwardFx.gameObject.SetActive(_thrust > 0.5f);
        }

        private void IntegrateAcceleration()
        {
            // Decay / Gain thrust
            _thrust = Mathf.SmoothDamp(_thrust, _thrustTarget, ref _thrustDerivative, engineLambda);

            var acceleration = _thrust * -transform.forward;
            accelerationChannel.Invoke(acceleration._x0z());

        }

        public void OnThrust(InputAction.CallbackContext context)
        {
            if (context.action.WasPressedThisFrame()) ScreenShake.Add(transform.position, 0, 0.5f);
            _thrustTarget = context.action.IsPressed() ? engineThrust : 0;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            ScreenShake.Add(transform.position, .2f, 0);
        }

        private void OrientShipFromMouse()
        {
            var mouse = Mouse.current.position.ReadValue();

            var ray = _camera.ScreenPointToRay(mouse);
            var plane = new Plane(Vector3.up, Vector3.zero);

            if (!plane.Raycast(ray, out var distance)) return;

            var forward = transform.forward;
            var point = ray.GetPoint(distance);
            var displacement = point - transform.position;

            // We want to dampen the displacement as we get closer to the ship.
            displacement = Vector3.Lerp(forward, displacement, displacement.magnitude / 4f);

            var direction = Vector3.Normalize(displacement);

            var bankAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
            bankAngle = Mathf.Clamp(bankAngle, -90, 90);
            var bankRotation = Quaternion.AngleAxis(-bankAngle, Vector3.forward);
            _rotationTarget = Quaternion.LookRotation(direction) * bankRotation;
        }
    }
}