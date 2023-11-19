//SPDX-License-Identifier: Unlicense
using UnityEngine;

namespace Features.Common
{
    public class TimeToLive : MonoBehaviour
    {
        [SerializeField]
        private float secondsUntilDestruction = 1f;
        private void Start() => Destroy(gameObject, secondsUntilDestruction);
    }
}
