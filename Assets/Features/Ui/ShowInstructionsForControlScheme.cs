using Tiger.Events;
using Tiger.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Ui
{
    using Log = Loggers.Create<ShowInstructionsForControlScheme>;
    
    public class ShowInstructionsForControlScheme : MonoBehaviour
    {
        [SerializeField] private Channel spawnChannel;
        [SerializeField] private Channel deathChannel;

        private PlayerInput _input;

        private void Awake()
        {
            Log.Logger.filterLogType = LogType.Warning;
            spawnChannel.subscribers.AddListener(OnSpawn);
            deathChannel.subscribers.AddListener(OnDeath);
        }

        private void OnDeath()
        {
            _input.onControlsChanged -= OnControlsChanged;
            _input.onDeviceRegained -= OnControlsChanged;
            ShowChild("Default");
        }

        private void OnSpawn()
        {
            _input = FindAnyObjectByType<PlayerInput>();
            _input.onDeviceRegained += OnControlsChanged;
            _input.onControlsChanged += OnControlsChanged;
            OnControlsChanged(_input);
        }

        private void OnControlsChanged(PlayerInput input)
        {
            var scheme = input.currentControlScheme;
            Log.Info($"Switching to {scheme}", this);

            ShowChild(scheme);
        }

        private void ShowChild(string scheme)
        {
            foreach (var child in transform.Children())
            {
                child.gameObject.SetActive(child.name == scheme);
            }
        }
    }
}

