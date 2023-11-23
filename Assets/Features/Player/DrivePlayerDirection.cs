using Tiger.Events;
using Tiger.Events.Concrete;
using Tiger.Swizzles;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Player
{
    public class DrivePlayerDirection : DataChannelEmitter<Vector3Channel, Vector3>
    {
        private void Update() => Emit(math.normalizesafe(transform.forward._x0z()));
    }
}
