using System.Collections;
using Tiger.Swizzles;
using UnityEngine;

namespace Features.Enemies
{
    public class RandomLaserFiring : MonoBehaviour
    {
        public float inaccuracy = 10;
        public Rigidbody laserPrefab;

        private IEnumerator Start()
        {
            while (Application.isPlaying)
            {
                yield return new WaitForSeconds(Random.Range(2f, 4f));

                var position = transform.localPosition;

                //Player is always at origin, but we could read the player position channel if needed.
                //That would require us to consider that a planar position, though. (e.g. swizzling _x0z, or otherwise)
                var fuzzyPlayerPosition = Random.insideUnitCircle.vx0y() * inaccuracy;
                var towardsPlayer = (fuzzyPlayerPosition - position).normalized; 
                var rotation = Quaternion.LookRotation(towardsPlayer, Vector3.up);
                
                Instantiate(laserPrefab, position, rotation, transform.parent);
            }
        }
    }
}
