using UnityEngine;

namespace Features.Space
{
    [ExecuteAlways]
    public class RotateSkybox : MonoBehaviour
    {
        private readonly int _shaderTransformPropertyId = Shader.PropertyToID("_SkyTransform");

        private void Update()
        {
            if (!RenderSettings.skybox) return;
            RenderSettings.skybox.SetMatrix(_shaderTransformPropertyId, Matrix4x4.Rotate(transform.rotation));
        }
    }
}
