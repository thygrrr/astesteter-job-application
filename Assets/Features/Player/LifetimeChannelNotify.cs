using Tiger.Events;
using UnityEngine;

namespace Features.Player
{
    public class LifetimeChannelNotify : MonoBehaviour
    {
        [SerializeField] private Channel onEnable;
        [SerializeField] private Channel onDisable;

        private void OnEnable() => onEnable.Invoke(gameObject);
        private void OnDisable() => onDisable.Invoke(gameObject);
    }
}
