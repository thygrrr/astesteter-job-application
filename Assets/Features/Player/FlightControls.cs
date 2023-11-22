using Channels.Concrete;
using Feature.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using Tiger.Swizzles;
using Tiger.Math;
using Tiger.ScreenShake;
using Unity.Mathematics;
using UnityEngine.Serialization;

namespace Features.Player
{
    using Debug = Loggers.Create<FlightControls>;
    
    public class FlightControls : MonoBehaviour, GameInputActions.IFlightActions
    {
        [SerializeField] [Tooltip("Channel to send acceleration data through")]
        private Vector3Channel accelerationChannel;

        [SerializeField] [Tooltip("Channel to send velocity data through")]
        private Vector3Channel velocityChannel;

        [SerializeField] [Tooltip("Channel to send current look/aim point data through")]
        private Vector3Channel lookChannel;

        [Header("View")] 
        [SerializeField] [Tooltip("Ship View to rotate and aim")]
        private Transform body;

        [Header("Thrust & Turning")]        [SerializeField] [Tooltip("Forward Acceleration (units/second²)")]
        private float enginePower = 20;

        [SerializeField] [Tooltip("Thrust Decay (half life)")]
        private float engineLambda = 0.1f;

        [SerializeField] [Tooltip("Rotation SmoothTime (half life)")]
        private float rotationLambda = 0.1f;

        [SerializeField] [Tooltip("Maximum Speed")]
        private float maxVelocity = 100;

        [FormerlySerializedAs("engineBrakeBoost")] [SerializeField] [Tooltip("Braking Boost (units/second²)")]
        private float engineBrakeFactor = 3;

        
        [Header("Screen Shake")] 
        [SerializeField] private float engineShakeTurnOn = 0.1f;
        [SerializeField] private float engineShakeRunning = 1f;

        [SerializeField] private GameObject forwardFx;

        private Camera _camera;

        private float _thrust;
        private float _thrustTarget;
        private float _thrustDerivative;

        private Quaternion _rotation;
        private Quaternion _rotationTarget;
        private Quaternion _rotationDerivative;

        private Vector3 _acceleration;
        private Vector3 _velocity;
        private Vector3 _steeringDirection;

        public void Awake()
        {
            Debug.Logger.filterLogType = LogType.Warning;
            _camera = Camera.main;
            forwardFx.SetActive(false);
        }

        private void Update()
        {
            OrientShipFromMouse();
            IntegrateRotation();
            
            IntegrateAcceleration(); // sic! physics operates in dynamic update for this project
            IntegrateVelocity();
        }

        private void IntegrateRotation()
        {
            //We rotate in dynamic update to make it extra smooth.
            _rotation = QuatEx.SmoothDamp(_rotation, _rotationTarget, ref _rotationDerivative, rotationLambda, Time.deltaTime);
            body.rotation = _rotation;
        }

        private void IntegrateAcceleration()
        {
            // Decay / Gain thrust
            _thrust = Mathf.SmoothDamp(_thrust, _thrustTarget, ref _thrustDerivative, engineLambda);

            // We use _steeringDirection instead of body.forward because we want user input to feel much snappier.
            var braking = math.smoothstep(0, -1, Vector3.Dot(_velocity.normalized, _steeringDirection));
            var boost = math.remap(-0.5f * math.SQRT2, 1, 1, engineBrakeFactor, braking); 
            var effectiveThrust = _thrust * boost;
            
            _acceleration = effectiveThrust * _steeringDirection;
            _acceleration = _acceleration._x0z();
            accelerationChannel.Emit(-_acceleration);

            if (_thrustTarget > 0) ScreenShake.Add(body.position, 0, engineShakeRunning * Time.deltaTime);
        }

        private void IntegrateVelocity()
        {
            _velocity += _acceleration * Time.deltaTime; 
            _velocity = Vector3.ClampMagnitude(_velocity, maxVelocity);
            velocityChannel.Emit(-_velocity);
        }

        public void OnThrust(InputAction.CallbackContext context)
        {
            switch (context.action.phase)
            {
                case InputActionPhase.Disabled:
                    //TODO: Engine Spool Down sound
                    Debug.Log("Thrust Disabled");
                    break;
                case InputActionPhase.Waiting:
                    //TODO: Engine Spool Up sound
                    Debug.Log("Thrust Waiting");
                    break;
                case InputActionPhase.Started:
                    Debug.Log("Thrust ON");
                    //TODO: Engine Afterburner ON Sound
                    _thrustTarget = enginePower;
                    forwardFx.gameObject.SetActive(true);
                    ScreenShake.Add(body.position, 0, engineShakeTurnOn);
                    break;
                case InputActionPhase.Performed:
                    break;
                case InputActionPhase.Canceled:
                    Debug.Log("Thrust OFF");
                    //TODO: Engine Afterburner OFF Sound
                    forwardFx.gameObject.SetActive(false);
                    _thrustTarget = 0;
                    break;
            } 
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }

        private void OrientShipFromMouse()
        {
            var mouse = Mouse.current.position.ReadValue();
            if (!_camera.pixelRect.Contains(mouse)) return;
            
            var ray = _camera.ScreenPointToRay(mouse);
            var plane = new Plane(Vector3.up, Vector3.zero);

            if (!plane.Raycast(ray, out var distance)) return;

            var look = ray.GetPoint(distance);
            lookChannel.Emit(look);
            
            var displacement = look - body.position;

            // We want to dampen the displacement as we get closer to the ship.
            var forward = body.forward;
            displacement = Vector3.Lerp(forward, displacement, displacement.magnitude / 3f);
            
            _steeringDirection = Vector3.Normalize(displacement);

            var bankAngle = Vector3.SignedAngle(forward, _steeringDirection, Vector3.up);
            bankAngle = Mathf.Clamp(bankAngle, -100, 100);
            var bankRotation = Quaternion.AngleAxis(-bankAngle, Vector3.forward);
            _rotationTarget = Quaternion.LookRotation(_steeringDirection) * bankRotation;
        }
    }
}