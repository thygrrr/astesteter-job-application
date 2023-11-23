using System.Collections;
using Features.Space;
using Tiger.Events;
using Tiger.Events.Concrete;
using Tiger.Swizzles;
using Tiger.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Enemies
{
    using Log = Loggers.Create<UfoSpawner>;
    
    public class UfoSpawner : DataChannelResponder<BoolChannel, bool>
    {
        [SerializeField]
        private GameObject[] ufoPrefabs;

        private WorldBounds _world;
        private GameObject _ufo;

        private void Awake()
        {
            _world = GetComponentInParent<WorldBounds>();
        }
    
        private IEnumerator Spawning()
        {
            while (Application.isPlaying)
            {
                //Wait until ufo has de-spawned (been destroyed)
                yield return new WaitUntil(() => !_ufo);
                
                var spawnRange = _world.bounds.size._x0z();
                var spawnCenter = _world.bounds.center;
                Vector3 spawnXZ = Random.insideUnitCircle.normalized._x0y() * 2 * spawnRange;

                //BUG: The bounds need to be expanded by the prefab's bounds, rarely you can see them popping in.
                var position = _world.bounds.ClosestPoint(spawnCenter + spawnXZ);
                var rotation = Quaternion.identity;
                Log.Info("It's night! Spawning UFOs!");
                _ufo = Instantiate(ufoPrefabs.Pick(), position, rotation, transform.parent);
            }
        }
    
        protected override void OnEvent(bool sunshine)
        {
            //Connecting to sunlight.
            if (!sunshine)
            {
                StartCoroutine(Spawning());
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }
}
