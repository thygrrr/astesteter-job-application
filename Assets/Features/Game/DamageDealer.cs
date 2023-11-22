//SPDX-License-Identifier: Unlicense

using UnityEngine;

namespace Features.Game
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField]
        private float damagePoints = 1;

        private void OnCollisionEnter(Collision other) => DealDamage(other);
        private void OnCollisionStay(Collision other) => DealDamage(other);

        private void DealDamage(Collision other)
        {
            if (other.gameObject.GetComponentInParent<DamageTaker>() is { } taker)
            {
                taker.ApplyDamage(damagePoints);
            }
        }
    }
}
