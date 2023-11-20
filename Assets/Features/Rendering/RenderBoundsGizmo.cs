//SPDX-License-Identifier: Unlicense

using UnityEngine;

namespace Features.Rendering
{
    [RequireComponent(typeof(Renderer))]
    public class RenderBounds : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var bounds = GetComponent<Renderer>().bounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

    }
}
