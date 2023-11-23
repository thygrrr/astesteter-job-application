using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Space
{
    public class PlayerDrivenGravity : DataChannelResponder<Vector3Channel, Vector3>
    {
        protected override void OnEvent(Vector3 data)
        {
         //Physics.gravity = data;   
        }
    }
}
