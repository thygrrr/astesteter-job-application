using System.Collections;
using Tiger.Swizzles;
using UnityEngine;

namespace Features.Enemies
{
    public class RandomLaserFiring : MonoBehaviour
    {
        public float muzzleVelocity = 50;
        public float accuracy = 7;
        public Rigidbody laserPrefab;

        private IEnumerator Start()
        {
            while (Application.isPlaying)
            {
                yield return new WaitForSeconds(Random.Range(2f, 4f));
            
                //Player is always at origin, but we could read the player position channel if needed.
                var towardsPlayer = Vector3.zero-(transform.position + (Vector3) Random.onUnitSphere._x0z() * accuracy).normalized;
                var rotation = Quaternion.LookRotation(towardsPlayer, Vector3.up);
                var laser = Instantiate(laserPrefab, transform.position, rotation, transform.parent);
                laser.velocity = laser.transform.forward * muzzleVelocity;
            }
        }
    }
}
