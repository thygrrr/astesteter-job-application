//SPDX-License-Identifier: Unlicense

using System.Collections.Generic;
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

        [SerializeField]
        private List<GameObject> remnantPrefabs;
        
        public void OnDeath()
        {
            for (var i = 0; i < remnantsToSpawn; i++)
            {
                //FIXME: Add planar random functions to LibTiger, this is pure jank. (non-uniform, type chaos, etc.)
                var length = math.remap(0, 1, minDisplacement, maxDisplacement, Random.value);
                var direction = math.remap(0, 1, -1, 1, new float3(Random.value, 0, Random.value));
                var displacement = math.normalize(direction) * length;

                var position = transform.position + (Vector3) displacement ;
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
