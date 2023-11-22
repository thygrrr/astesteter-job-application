using Channels.Concrete;
using Tiger.Events;
using UnityEngine;

namespace Features.Game
{
    using Log = Loggers.Create<AccelerationRigidbodyResponder>;
    [RequireComponent(typeof(Rigidbody))]
    public class AccelerationRigidbodyResponder : DataChannelResponder<Vector3Channel, Vector3>
    {
        private Rigidbody _body;
        protected override void AwakeOverride()
        {
            _body = GetComponent<Rigidbody>();
        }
        
        protected override void OnEvent(Vector3 data) => _body.AddForce(data, ForceMode.Acceleration);
    }
}
