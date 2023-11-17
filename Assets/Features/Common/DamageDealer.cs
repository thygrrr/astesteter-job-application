using UnityEngine;

namespace Features.Common
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField]
        private int damagePoints = 1;
        
        public void DealDamage(ref int hitPoints)
        {
            hitPoints = Mathf.Max(hitPoints - damagePoints, 0);
        }
    }
}
