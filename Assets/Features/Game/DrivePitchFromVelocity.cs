using Tiger.Events;
using Tiger.Events.Concrete;
using UnityEngine;

namespace Features.Game
{
    public class DrivePitchFromVelocity : DataChannelResponder<Vector3Channel, Vector3>
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private float speedScale = 0.01f;
        [SerializeField] private float basePitch = 1f;

        private void Awake()
        {
            source.pitch = basePitch;
        }

        protected override void OnEvent(Vector3 data)
        {
            source.pitch = basePitch + data.magnitude * speedScale;
        }
    }
}
