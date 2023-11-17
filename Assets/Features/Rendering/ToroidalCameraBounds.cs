using UnityEngine;

namespace Features.Rendering
{
    [ExecuteAlways]
    public class ToroidalCameraBounds : MonoBehaviour
    {
        [SerializeField] private Bounds bounds = new (Vector3.zero, Vector3.one * 10f);

        readonly int _toroidalCameraBounds = Shader.PropertyToID("_ToroidalCameraBounds");
        // Update is called once per frame
        private void Update()
        {
            Shader.SetGlobalVector(_toroidalCameraBounds, bounds.extents);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
