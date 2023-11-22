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
            print("Damage");
            
            if (other.gameObject.GetComponentInParent<DamageTaker>() is not { } taker) return;
            
            print("found taker");
            taker.ApplyDamage(damagePoints);
        }
    }
}
