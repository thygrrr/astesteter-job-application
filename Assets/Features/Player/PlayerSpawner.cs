using Feature.Ui;
using Tiger.Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Channel playerSpawned;

    private GameObject _player;
    
    private GameInputActions _input;
    private void Awake()
    {
        _input = new GameInputActions();
        _input.UI.Spawn.performed += SpawnPlayer;
    }
    
    private void OnEnable() => _input?.UI.Enable();
    
    private void OnDisable() => _input?.UI.Disable();

    private void SpawnPlayer(InputAction.CallbackContext ctx)
    {
        if (_player != null) return;
        _player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform.parent);
    }
}
