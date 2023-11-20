using Feature.Ui;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Features.Player
{
    public class ShipThrusterControls : MonoBehaviour, GameInputActions.IPlayerActions
    {
        [SerializeField] [Tooltip("Forward thrust")]
        private float forward = 8;
        [SerializeField] [Tooltip("Reverse thrust")]
        private float backward = -2f; 

        [SerializeField] [Tooltip("Local X-Axis thrust (right, left)")]
        private float strafing = 4f;
        
        [SerializeField] 
        private GameObject forwardFx;
        
        private float3 minThrust => new float3(-strafing, 0, backward);
        private float3 maxThrust => new float3(strafing, 0, forward);
        
        private Rigidbody _body;
        private Camera _camera;

        private Vector3 _thrust;

        public void Awake()
        {
            _camera = Camera.main;
            forwardFx.SetActive(false);
            _body = GetComponentInParent<Rigidbody>();
            _body.rotation = Quaternion.Euler(0, Random.Range(-360, 360), 0);
        }

        private void FixedUpdate()
        {
            _body.AddForce(_thrust, ForceMode.Force);
            forwardFx.gameObject.SetActive(math.dot(transform.forward, _thrust) > 0.8f);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            // Clamp input
            var input = context.action.ReadValue<Vector2>();
            var moveInputVector = Vector3.ClampMagnitude(new Vector3(input.x, 0f, input.y), 1f);
            moveInputVector = math.remap(-Vector3.one, Vector3.one, minThrust, maxThrust, moveInputVector);
            var viewForward = _camera.transform.forward;
            var viewUp = _camera.transform.up;

            // Calculate camera direction and rotation on the character plane
            var cameraPlanarDirection = Vector3.ProjectOnPlane(viewForward, Vector3.up).normalized;
            if (cameraPlanarDirection.sqrMagnitude <= 0.01f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(viewUp, Vector3.up).normalized;
            }

            var cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Vector3.up);
            _thrust = cameraPlanarRotation * moveInputVector;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }

        public void OnFire(InputAction.CallbackContext context)
        {
        }
    }
}
