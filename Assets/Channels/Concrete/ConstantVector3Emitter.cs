using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Channels.Concrete
{
    public class ConstantVector3Emitter : DataChannelEmitter<Vector3Channel, Vector3>
    {
        public Vector3 value;

        private void Update()
        {
            Emit(value);
        }
    }
}
