using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Space
{
    public class PlayerDrivenGravity : DataChannelResponder<Vector3Channel, Vector3>
    {
        //Doesn't work with particles in Unity 2023. IKR?
        protected override void OnEvent(Vector3 data)
        {
#if !UNITY_2023_1_OR_NEWER
            Physics.gravity = data;
#endif
        }

        private void Awake()
        {
#if !UNITY_2023_1_OR_NEWER
            Physics.gravity = Vector3.zero;
#endif
        }

        private void OnDestroy()
        {
#if !UNITY_2023_1_OR_NEWER
            Physics.gravity = Vector3.zero;
#endif
        }
    }
}
