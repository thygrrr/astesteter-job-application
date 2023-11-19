//SPDX-License-Identifier: Unlicense
using UnityEngine;

namespace Features.Common
{
    [RequireComponent(typeof(Collider))]
    public class DamageTaker : MonoBehaviour
    {
        [SerializeField]
        private int hitPoints = 1;
        
        private void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.GetComponent<DamageDealer>() is not { } damageDealer) return;
            
            damageDealer.DealDamage(ref hitPoints);
            if (hitPoints <= 0) Destroy(gameObject);
        }
    }
}
