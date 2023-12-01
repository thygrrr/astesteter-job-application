using Tiger.Events;
using Tiger.Events.Concrete;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class DriveEngineSoundFromAcceleration : DataChannelResponder<Vector3Channel, Vector3>
{
    [FormerlySerializedAs("source")] [SerializeField]
    private AudioSource thrusterSource;

    [SerializeField]
    private AudioSource rumbleSource;

    [SerializeField] private float maxAccel = 200f;
    [SerializeField] private float baseVolume = 0f;
    [SerializeField] private float basePitch = 3f;
    [SerializeField] private float pitchScale = -2f;
    [SerializeField] private float volumeScale = 1f;
    [SerializeField] private float upThrustLambda = 0.2f;
    [SerializeField] private float downThrustLambda = 0.1f;
    [SerializeField] private float upRumbleLambda = 0.3f;
    [SerializeField] private float downRumbleLambda = 0.2f;
    

    private float _goal;
    private float _smoothThrust;
    private float _derivativeThrust;
    private float _smoothRumble;
    private float _derivativeRumble;
    
    private void Update()
    {
        _smoothThrust = _smoothThrust <= _goal
            ? Mathf.SmoothDamp(_smoothThrust, _goal, ref _derivativeThrust, downThrustLambda)
            : Mathf.SmoothDamp(_smoothThrust, _goal, ref _derivativeThrust, upThrustLambda);

        _smoothRumble = _smoothRumble <= _goal
            ? Mathf.SmoothDamp(_smoothRumble, _goal, ref _derivativeRumble, downRumbleLambda)
            : Mathf.SmoothDamp(_smoothRumble, _goal, ref _derivativeRumble, upRumbleLambda);
        
        thrusterSource.volume = baseVolume + _goal * volumeScale;
        thrusterSource.pitch = basePitch + _smoothThrust * pitchScale;
        rumbleSource.volume = baseVolume + _smoothRumble * volumeScale;
    }
    
    protected override void OnEvent(Vector3 data)
    {
        _goal = math.smoothstep(0, maxAccel, data.magnitude);
    }
}
