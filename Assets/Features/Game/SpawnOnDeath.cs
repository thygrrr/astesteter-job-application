//SPDX-License-Identifier: Unlicense

using System.Collections.Generic;
using Tiger.Events;
using Tiger.Util;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Game
{
    using Log = Loggers.Create<SpawnOnDeath>;
    
    public class SpawnOnDeath : MonoBehaviour, IOnDeath
    {
        [Header("On Death")] 
        [SerializeField] private int remnantsToSpawn = 2;
        [SerializeField] private bool randomizeRotation = true;

        [SerializeField] private float minDisplacement = 0;
        [SerializeField] private float maxDisplacement = 0;

        // NB, with the new coordinate system, this is always at origin.
        // otherwise, we can read the player position from the appropriate channel
        // (using DataChannel<T>.value, not a subscription).
        private Vector3 playerPosition => Vector3.zero; 
        
        [Header("Remnants")]
        [SerializeField]
        private List<GameObject> remnantPrefabs;
        
        public void OnDeath() 
        {
            for (var i = 0; i < remnantsToSpawn; i++)
            {
                //TODO: Add planar & mapped random functions to LibTiger, this is pure jank. (non-uniform, type chaos, etc.)

                //Split perpendicular to player direction. Nobody wants an Asteroid to the face if they can help it.
                var length = maxDisplacement * Random.value + minDisplacement;

                var direction = Vector3.Normalize(transform.position - playerPosition);
                var ccw = i % 2 == 0 ? Vector3.up : Vector3.down;
                var displacement = Vector3.Cross(direction, ccw) * length; 
                
                var position = transform.position + displacement ;
                var rotation = randomizeRotation ? Random.rotationUniform : Quaternion.identity;
                
                Instantiate(remnantPrefabs.Pick(), position, rotation, transform.parent);
            }
        }

        private void OnValidate()
        {
            if (remnantPrefabs.Count == 0 && remnantsToSpawn > 0)
            {
                Log.Error($"Behaviour will want to spawn {remnantsToSpawn} remnants, and therefore needs at least 1 prefab or will crash at runtime.", this);
            }
        }
    }
}
