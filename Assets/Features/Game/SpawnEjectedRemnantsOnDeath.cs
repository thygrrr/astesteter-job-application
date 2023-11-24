//SPDX-License-Identifier: Unlicense

using System.Collections.Generic;
using Features.Motion;
using Tiger.Swizzles;
using Tiger.Util;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Game
{
    using Log = Loggers.Create<SpawnEjectedRemnantsOnDeath>;
    
    [RequireComponent(typeof(ProvideIntegration))]
    public class SpawnEjectedRemnantsOnDeath : MonoBehaviour, IOnDeath
    {
        [Header("On Death")] 
        [SerializeField] private int remnantsToSpawn = 0;
        [SerializeField] private bool randomizeRotation = true;

        [Header("Ejection")]
        [SerializeField] private float minEjectionSpeed = 5;
        [SerializeField] private float maxEjectionSpeed = 10;
        [SerializeField] private float randomDirectionSpeed = 10;
        [SerializeField] private float minDisplacement = 0;
        [SerializeField] private float maxDisplacement = 0;

        // NB, with the new coordinate system, this is always at origin.
        // otherwise, we can read the player position from the appropriate channel
        // (using DataChannel<T>.value, not a subscription).
        private Vector3 playerPosition => Vector3.zero; 
        
        [Header("Remnants")]
        [SerializeField]
        private List<ProvideIntegration> remnantPrefabs;

        private ProvideIntegration _ownIntegrator;

        private void Awake()
        {
            _ownIntegrator = GetComponent<ProvideIntegration>();
        }

        public void OnDeath() 
        {
            for (var i = 0; i < remnantsToSpawn; i++)
            {
                
                //Split direction
                Vector3 direction;
                
                //Split perpendicular to player direction. Nobody wants an Asteroid to the face if they can help it.
                var splitting = remnantsToSpawn > 1;
                if (splitting)
                {
                    //Perpendicular split
                    var directionToPlayer = Vector3.Normalize(transform.localPosition - playerPosition);
                    var ccw = i % 2 == 0 ? Vector3.up : Vector3.down;
                    direction = Vector3.Cross(directionToPlayer, ccw);
                }
                else
                {
                    //Single remnant, displace randomly
                    direction = Random.onUnitSphere.vxz0().normalized;
                    //TODO: Add planar & mapped random functions to LibTiger, this is pure jank. (non-uniform, type chaos, etc.)
                }

                var magnitude = Random.Range(minDisplacement, maxDisplacement);
                var displacement = direction * magnitude;
                var position = transform.position + displacement;
                var rotation = randomizeRotation ? Random.rotationUniform : Quaternion.identity;

                var remnant = Instantiate(remnantPrefabs.Pick(), position, rotation, transform.parent);
                remnant.velocity = _ownIntegrator.velocity;
                
                var ejectionVelocity = splitting ? direction * Random.Range(minEjectionSpeed, maxEjectionSpeed) : Vector3.zero;
                ejectionVelocity += Random.insideUnitSphere.vx0z() * randomDirectionSpeed;
                remnant.velocity += (float3) ejectionVelocity; //repeated for readability
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
