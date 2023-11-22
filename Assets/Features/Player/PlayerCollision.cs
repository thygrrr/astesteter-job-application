using Features.Game;
using UnityEngine;

namespace Features.Player
{
    public class PlayerCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            //Punch through whatever we're hitting.
            if (other.gameObject.GetComponentInParent<DamageTaker>() is { } taker)
            {
                taker.ApplyDamage(9001); //over 9000
            }
        
            // Normally, Killables handle their own collisions, but I haven't refactored this for the player enough yet.
            // The hitbox is pretty far down, but probably it can be moved up to where the KillablePlayer component is.
            GetComponentInParent<KillablePlayer>().CrashAndBurn();
        }
    }
}
