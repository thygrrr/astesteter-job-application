using UnityEngine;

namespace Features.Space
{
    public class SlowRotate : MonoBehaviour
    {
        [Tooltip("Degrees per second")]
        [SerializeField]
        private Vector3 speed = new(0, 2,0 );

        private void Update() => transform.Rotate(speed * Time.deltaTime, UnityEngine.Space.Self);
    }
}
