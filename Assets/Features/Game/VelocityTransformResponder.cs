using Channels.Concrete;
using Tiger.Events;
using UnityEngine;

namespace Features.Game
{
    using Log = Loggers.Create<VelocityTransformResponder>;
    public class VelocityTransformResponder : DataChannelResponder<Vector3Channel, Vector3>
    {
        protected override void OnEvent(Vector3 data)
        {
            transform.Translate(data * Time.deltaTime, UnityEngine.Space.World);            
        }
    }
}
