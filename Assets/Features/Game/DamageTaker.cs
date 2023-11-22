//SPDX-License-Identifier: Unlicense

using UnityEngine;

namespace Features.Game
{
    public class DamageTaker : Killable
    {
        [SerializeField]
        private int hitPoints = 1;

        public void ApplyDamage(int damage)
        {
            hitPoints -= damage;
            if (hitPoints <= 0) Die();
        }
    }
}
