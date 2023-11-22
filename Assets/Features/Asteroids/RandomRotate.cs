using UnityEngine;

namespace Features.Asteroids
{
    public class RandomRotate : MonoBehaviour
    {
        [SerializeField] [Tooltip("Degrees per second")]
        private float minSpeed = 5;
        [SerializeField] [Tooltip("Degrees per second")]
        private float maxSpeed = 10;

        private Vector3 _angularVelocity;
        
        private void Awake()
        {
            transform.localRotation = Random.rotationUniform;
            _angularVelocity = Random.onUnitSphere * Random.Range(minSpeed, maxSpeed);
        }

        private void Update()
        {
            transform.Rotate(_angularVelocity * Time.deltaTime, UnityEngine.Space.World);   
        }
    }
}
