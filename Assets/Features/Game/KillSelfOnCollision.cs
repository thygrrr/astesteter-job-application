//SPDX-License-Identifier: Unlicense

using UnityEngine;

namespace Features.Game
{
    public class KillSelfOnCollision : Killable
    {
        private void OnCollisionEnter(Collision _) => Die();
    }
}
