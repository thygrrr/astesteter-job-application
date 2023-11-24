//SPDX-License-Identifier: Unlicense

using System.Collections;
using UnityEngine;

namespace Features.Game
{
    public class TimeToLive : Killable
    {
        [SerializeField] private float secondsUntilDestruction = 1f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(secondsUntilDestruction);
            Die();
        }
    }
}
