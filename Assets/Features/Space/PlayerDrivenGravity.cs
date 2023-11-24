using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Space
{
    public class PlayerDrivenGravity : DataChannelResponder<Vector3Channel, Vector3>
    {
        //Doesn't work with particles in Unity 2023. IKR?
#if !UNITY_2023_1_OR_NEWER
        protected override void OnEvent(Vector3 data)
        {
            Physics.gravity = data;  
        }

        private void Awake()
        {
            Physics.gravity = Vector3.zero;  
        }

        private void OnDestroy()
        {
            Physics.gravity = Vector3.zero;  
#endif
        }
    }
}
