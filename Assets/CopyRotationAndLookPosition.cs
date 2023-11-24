using Tiger.Events;
using UnityEngine;

public class CopyRotationAndLookPosition : MonoBehaviour, IHierarchicalUpdate
{
    [SerializeField] private Transform source;
    
    void IHierarchicalUpdate.HierarchicUpdate(float deltaTime)
    {
        transform.rotation = source.rotation;
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = new Ray(source.position, source.forward);
        transform.position = plane.Raycast(ray, out var distance) ? ray.GetPoint(distance) : source.position;
    }

    private void OnValidate()
    {
        if (!source) Debug.LogWarning("CopyTransform source is not set.", this);
    }
}
