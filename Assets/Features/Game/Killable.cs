using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Game
{
    [DisallowMultipleComponent]
    public class Killable : MonoBehaviour
    {
        private IEnumerable<IOnDeath> dependents => GetComponentsInChildren<IOnDeath>();
        
        private bool _dying;

        protected void Die()
        {
            if (!_dying) StartCoroutine(DeathProcedure());   
        } 
    
        private IEnumerator DeathProcedure()
        {
            _dying = true;
            yield return new WaitForEndOfFrame();
            
            gameObject.SetActive(false);

            foreach (var child in dependents)
            {
                child.OnDeath();
            }

            Destroy(gameObject);
        }
    }
}
