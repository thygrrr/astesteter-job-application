//SPDX-License-Identifier: Unlicense

using System.Collections.Generic;
using Tiger.Util;
using UnityEngine;

namespace Features.Game
{
    using Log = Loggers.Create<SpawnOnDeath>;
    
    public class SpawnOnDeath : MonoBehaviour, IOnDeath
    {
        [Header("On Death")] 
        [SerializeField] private int remnantsToSpawn = 2;
        [SerializeField] private bool randomizeRotation = true;

        [SerializeField]
        private List<GameObject> remnantPrefabs;
        
        public void OnDeath()
        {
            for (var i = 0; i < remnantsToSpawn; i++)
            {
                var rotation = randomizeRotation ? Random.rotationUniform : Quaternion.identity;
                Instantiate(remnantPrefabs.Pick(), transform.position, rotation, transform.parent);
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
