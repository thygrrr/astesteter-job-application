using UnityEngine;

namespace Features.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerDrivenGravity : MonoBehaviour
    {
        private Rigidbody _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Physics.gravity = new Vector3(5, 0, 0);
            //_body.GetAccumulatedForce();
        }
    }
}
