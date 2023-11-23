using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Player
{
    public class DrivePlayerDirection : DataChannelEmitter<Vector3Channel, Vector3>
    {
        private void Update()
        {
            var planar = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            Emit(math.normalizesafe(planar));   
        }
    }
}
