using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiger.Audio; 

public class OnAwakePlayOneShotAudio : MonoBehaviour
{
    [SerializeField] private AudioEvent audioEvent;
    
    private void Awake()
    {
        audioEvent.PlayOneShot(transform.position);
    }
}
