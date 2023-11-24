using Tiger.Events;
using UnityEngine;

public class CopyRotation : MonoBehaviour, IHierarchicalUpdate
{
    [SerializeField] private Transform source;
    
    void IHierarchicalUpdate.HierarchicUpdate(float deltaTime)
    {
        transform.rotation = source.rotation;
    }

    private void OnValidate()
    {
        if (!source) Debug.LogWarning("CopyTransform source is not set.", this);
    }
}
