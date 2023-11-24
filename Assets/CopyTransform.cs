using Tiger.Events;
using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;

public class CopyTransform : MonoBehaviour, IHierarchicalUpdate
{
    [SerializeField] private Transform source;
    [SerializeField] private bool copyPosition = false;
    [SerializeField] private bool copyRotation = true;
    [SerializeField] private float3 angles = 1;
    

    void IHierarchicalUpdate.HierarchicUpdate(float deltaTime)
    {
        if (copyPosition && copyRotation)
        {
            var euler = source.rotation.eulerAngles.fxyz() * angles;
            transform.SetPositionAndRotation(source.position, Quaternion.Euler(euler));
            return;
        }
        
        if (copyPosition)
        {
            transform.position = source.position;
            return;
        }
        
        if (copyRotation)
        {
            var euler = source.rotation.eulerAngles.fxyz() * angles;
            transform.rotation = Quaternion.Euler(euler);
            return;
        }
    }

    private void OnValidate()
    {
        if (!source) Debug.LogWarning("CopyTransform source is not set.", this);
    }
}
