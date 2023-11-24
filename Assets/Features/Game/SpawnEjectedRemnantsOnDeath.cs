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
    using Log = Loggers.Create<SpawnPerpendicularRemnantsOnDeath>;
    
    [RequireComponent(typeof(ProvideIntegration))]
    public class SpawnPerpendicularRemnantsOnDeath : MonoBehaviour, IOnDeath
    {
        [Header("On Death")] 
        [SerializeField] private int remnantsToSpawn = 0;
        [SerializeField] private bool randomizeRotation = true;

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
                //TODO: Add planar & mapped random functions to LibTiger, this is pure jank. (non-uniform, type chaos, etc.)
                
                //Split perpendicular to player direction. Nobody wants an Asteroid to the face if they can help it.
                Vector3 ejection;
                var amount = Random.Range(minDisplacement, maxDisplacement);
                if (remnantsToSpawn < 2 || amount == 0)
                {
                    //Single remnant, don't do perpendicular.
                    ejection  = Random.onUnitSphere._xz0();
                }
                else
                {
                    var directionToPlayer = Vector3.Normalize(transform.position - playerPosition);
                    var ccw = i % 2 == 0 ? Vector3.up : Vector3.down;
                    ejection = Vector3.Cross(directionToPlayer, ccw);
                }

                var displacement = ejection * amount;
                var position = transform.position + displacement;
                var rotation = randomizeRotation ? Random.rotationUniform : Quaternion.identity;

                var ejectionVelocity = amount > 0 ? ejection * math.length(_ownIntegrator.velocity) : Vector3.zero;

                var remnant = Instantiate(remnantPrefabs.Pick(), position, rotation, transform.parent);
                remnant.velocity = _ownIntegrator.velocity + (float3) ejectionVelocity;
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
