using System.Collections.Generic;
using UnityEngine;

namespace Features.Common
{
    public class SpawnOnDestroy : MonoBehaviour
    {
        [Header("OnDestroy")] [SerializeField]
        private int remnantsToSpawn = 2;

        [SerializeField]
        private List<GameObject> remnantPrefabs;
        
        private void OnDestroy()
        {
            //FIXME: Not allowed to do this :D
            if (!Application.isPlaying) return;
            for (var i = 0; i < remnantsToSpawn; i++)
            {
                //Instantiate(remnantPrefabs.Pick(), transform.position, Random.rotationUniform);
            }
        }

        private void OnValidate()
        {
            if (remnantPrefabs.Count == 0 && remnantsToSpawn > 0)
            {
                Debug.LogError($"SplitOnDestroy will want to spawn {remnantsToSpawn} remnants, and therefore needs at least 1 prefab", this);
            }
        }
    }
}
