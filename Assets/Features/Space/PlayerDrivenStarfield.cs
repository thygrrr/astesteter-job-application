using Channels.Concrete;
using Tiger.Events;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class PlayerDrivenStarfield : DataChannelResponder<Vector3Channel, Vector3>
{
    private VisualEffect _vfx;
    private readonly int _speed = Shader.PropertyToID("_velocity");
    
    private void Start() => _vfx = GetComponent<VisualEffect>();
    protected override void OnEvent(Vector3 data) => _vfx.SetVector3(_speed, data);
}
