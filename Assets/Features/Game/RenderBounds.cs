//SPDX-License-Identifier: Unlicense

using UnityEngine;

namespace Features.Game
{
    public class RenderBounds : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var bounds = GetComponent<MeshRenderer>().bounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

    }
}
