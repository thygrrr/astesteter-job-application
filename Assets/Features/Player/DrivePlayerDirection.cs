using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

public class DrivePlayerDirection : DataChannelEmitter<Vector3Channel, Vector3>
{
    void Update() => Emit(transform.forward);
}
