using Tiger.Events;
using UnityEngine;
using Log = Loggers.Create<GameWorld>;
public class GameWorld : MonoBehaviour
{
    [SerializeField]
    private Rigidbody player;
    
    public DataChannel<Vector3> playerPosition;
    public DataChannel<Vector3> playerVelocity;
    
    public void FixedUpdate()
    {
        playerPosition.Invoke(player.position);
        playerVelocity.Invoke(player.velocity);
    }

    private void OnValidate()
    {
        if (!player) Log.Error("Player is not set", this);
        if (!playerPosition) Log.Error("Channel playerPosition is not connected", this);
        if (!playerVelocity) Log.Error("Channel playerPosition is not connected", this);
    }
}
