using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Player
{
    public class RotateTowardsPlayerDirection : DataChannelResponder<Vector3Channel, Vector3>
    {
        protected override void OnEvent(Vector3 data)
        {
            transform.LookAt(data, Vector3.up);
        }
    }
}
