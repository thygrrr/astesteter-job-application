//SPDX-License-Identifier: Unlicense

using UnityEngine;

namespace Features.Game
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField]
        private int damagePoints = 1;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponentInParent<DamageTaker>() is { } taker)
            {
                taker.ApplyDamage(damagePoints);
            }
        }
    }
}
